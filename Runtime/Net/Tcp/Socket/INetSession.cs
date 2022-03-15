using System;

namespace UnityLib.Net
{
    public interface INetSession<TMessage>
    {
        public delegate TMessage Deserialize(ReadOnlySpan<byte> data);
        public delegate ReadOnlySpan<byte> Serialize(TMessage msg);

        public void SendMessage(TMessage message);
        public event Action<TMessage> onReceiveMessage;
        public event Serialize serialize;
        public event Deserialize deserialize;
        public bool IsReceive { get; }
        public void StartReceive();
        public void StopReceive();
        public void Connect(string host, int port);
    }
}