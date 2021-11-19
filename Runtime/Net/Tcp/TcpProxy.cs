using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace UnityLib.Net
{
    public struct NetMsg
    {
        private int m_id;
        private byte[] m_data;

        public int Id => m_id;
        public byte[] Data => m_data;

        public static implicit operator byte[](NetMsg msg)
        {
            if (msg.m_data == null)
                throw new ArgumentException($"msg data is null !");
            var buffer = new byte[sizeof(int) * 2 + msg.m_data.Length];
            Array.Copy(BitConverter.GetBytes(buffer.Length), buffer, sizeof(int));
            Array.Copy(BitConverter.GetBytes(msg.m_id), 0, buffer, sizeof(int), sizeof(int));
            Array.Copy(msg.m_data, 0, buffer, sizeof(int) * 2, msg.Data.Length);
            return msg;
        }
    }

    public class TcpProxy : TcpClient
    {
        private const int LENGTHSIZE = 4;

        private Memory<byte> dataMemory = new byte[1024];
        private Memory<byte> lengthMemory = new byte[LENGTHSIZE];

        private int size = -1;

        #region 发消息
        private CancellationTokenSource sendToken = new CancellationTokenSource();
        private bool runSend;
        private ConcurrentQueue<byte[]> sendmsgQueue = new ConcurrentQueue<byte[]>();

        public void TryAdd(byte[] msgdata)
        {
            if (msgdata == null)
            {
                Debug.LogException(new ArgumentNullException(nameof(msgdata)));
                return;
            }
            sendmsgQueue.Enqueue(msgdata);
            TrySend();
        }

        private async void TrySend()
        {
            if (runSend)
                return;
            runSend = true;
            if (sendmsgQueue.TryDequeue(out var msgdata))
            {
                await GetStream().WriteAsync(new ReadOnlyMemory<byte>(BitConverter.GetBytes(msgdata.Length)), sendToken.Token);
                await GetStream().WriteAsync(new ReadOnlyMemory<byte>(msgdata), sendToken.Token);
            }
            runSend = false;
        }

        #endregion

        #region 收消息
        private CancellationTokenSource receiveToken;
        private ConcurrentQueue<byte[]> receiveQueue = new ConcurrentQueue<byte[]>();

        public async void Receive()
        {

            Debug.Log("开启监听!");
            if (receiveToken != null)
                throw new Exception("已经开启监听");
            receiveToken = new CancellationTokenSource();
            
            while (!receiveToken.IsCancellationRequested)
            {
                await ReadLenth();
                await ReadData();
            }
            receiveToken = null;
            
        }

        private async Task ReadLenth()
        {
            var l = await GetStream().ReadAsync(lengthMemory, receiveToken.Token);
            Debug.Assert(l == LENGTHSIZE, "读写消息长度错误");
            size = BitConverter.ToInt32(lengthMemory.Span);
        }

        private async Task ReadData()
        {
            if (dataMemory.Length < size)
                dataMemory = new byte[size];
            var l = await GetStream().ReadAsync(dataMemory);
            Debug.Assert(l == size, "读写消息体错误");
            receiveQueue.Enqueue(dataMemory.Slice(0, l).ToArray());
        }

        public bool TryDequeue(out byte[] result)
        {
            return receiveQueue.TryDequeue(out result);
        }
        #endregion
    }

    public static class TcpProxy_Ex
    {
        public class RPCAwaiter : IAwaiter<RPCAwaiter>, IAsync<byte[]>,IDisposable
        {
            private event Action callbackEvent;

            private bool isCompleted = false;
            public bool IsCompleted
            {
                get => isCompleted; 
                set
                {
                    isCompleted = value;
                    if (isCompleted)
                        callbackEvent.Invoke();
                }
            }

            private byte[] data;
            public void SetResult(byte[] data)
            { 
                this.data = data;
            }

            public RPCAwaiter GetAwaiter()
            {
                return this;
            }

            public byte[] GetResult()
            {
                return data;
            }

            public void OnCompleted(Action continuation)
            {
                callbackEvent += continuation;
            }

            public void Dispose()
            {
                data = null;
                isCompleted = false;
                callbackEvent.Clone();
            }
        }

        public static RPCAwaiter RPCMsg(this TcpProxy tcp, byte[] data)
        {
            tcp.TryAdd(data);
            var result = new RPCAwaiter();
            NetMgr.Instance.netMsgs.Add(result);
            return result;
        }
    }
}