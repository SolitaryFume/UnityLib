using System.Net.Sockets;
using System;
using System.Threading;
using System.Collections.Generic;
using Proto;

namespace UnityLib.Net
{
    public class SocketProxy<T> : NetworkStream
    {
        public delegate byte[] MessageSerialize(T msg);
        public delegate T MessageDeSerialization(Span<byte> data);

        public event MessageSerialize messageSerialize;
        public event MessageDeSerialization messageDeSerialization;
        public event Action<T> messageHandle;

        private CancellationTokenSource receiveToken;
        public SocketProxy(Socket socket) : base(socket)
        {
        }

        public async void StartReadMessage()
        {
            if (receiveToken != null)
            {
                return;
            }

            receiveToken = new CancellationTokenSource();
            while (!receiveToken.IsCancellationRequested)
            {
                await ReadAsync(lengthMemory, receiveToken.Token);
                var l = BitConverter.ToInt32(lengthMemory.Span);
                if (l > maxSize)
                {
                    data = new byte[l];
                }
                var dataMemory = new Memory<byte>(data, 0, l);
                await ReadAsync(dataMemory, receiveToken.Token);
                if (messageDeSerialization == null)
                {
                    throw new Exception("没有反序列化器");
                }
                var msg = messageDeSerialization(data);
                OnReceive(msg);
            }
        }

        public void StopReadMessage()
        {
            receiveToken?.Cancel();
            receiveToken = null;
        }

        public async void SendMessage(T msg)
        {
            if (messageSerialize == null)
            {
                throw new Exception("没有列化器");
            }
            var data = messageSerialize(msg);
            await this.WriteAsync(data);
        }

        private int maxSize = 1024;
        private byte[] data = new byte[1024];
        private Memory<byte> lengthMemory = new byte[4];

        private void OnReceive(T message)
        {
            messageHandle?.Invoke(message);
        }
    }

    public class NetManagerHandle
    {
        public static void MessageHandle(IMessage message)
        {

        }
    }
}