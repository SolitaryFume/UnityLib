using Proto;
using System;
using System.Net.Sockets;
using System.Threading;

namespace UnityLib.Net
{

    public class UdpServer : INetSession<IMessage>
    {
        private UdpClient m_udp;
        private CancellationTokenSource m_ReceiveCTS;

        public UdpServer()
        { 
            m_udp = new UdpClient();
        }

        ~UdpServer()
        {
            m_udp.Close();
            m_udp = null;
        }

        public bool IsReceive => throw new NotImplementedException();

        public event Action<IMessage> onReceiveMessage;
        public event INetSession<IMessage>.Serialize serialize;
        public event INetSession<IMessage>.Deserialize deserialize;

        public void SendMessage(IMessage message)
        {
            Byte[] data = serialize.Invoke(message).ToArray();
            //var l = msgDt.Length + sizeof(int);
            //var data = new byte[l];
           
            m_udp.SendAsync(data, data.Length);
        }

        private void OnReceiveMessage(IMessage message)
        {
            onReceiveMessage?.Invoke(message);
        }

        public async void StartReceive()
        {
            if (m_ReceiveCTS != null && m_ReceiveCTS.IsCancellationRequested)
                return;
            m_ReceiveCTS = new CancellationTokenSource();
            while (!m_ReceiveCTS.IsCancellationRequested)
            {
                var result = await m_udp.ReceiveAsync();
                OnReceive(result.Buffer);
            }
        }

        private void OnReceive(ReadOnlySpan<byte> buffer)
        {

            if (BitConverter.ToInt32(buffer.Slice(0, sizeof(int))) == buffer.Length)
            {
                var msg = deserialize(buffer.Slice(sizeof(int)));
                OnReceiveMessage(msg);
            }
            else
            {
                Debug.LogError("buffer isn't valid data!");
            }
        }

        public void StopReceive()
        {
            if (m_ReceiveCTS == null)
                return;
            m_ReceiveCTS.Cancel();
        }

        public void Connect(string host, int port)
        {
            m_udp.Connect(host, port);
        }
    }
}