using Proto;
using System.Net.Sockets;
using UnityEngine;

namespace UnityLib.Net
{
    public class TcpServer
    {
        public readonly TcpClient client;
        private SocketProxy<IMessage> socketProxy;

        public TcpServer(string host, int port)
        {
            client = new TcpClient();
            client.Connect(host, port);
            socketProxy = new SocketProxy<IMessage>(client.Client);
        }
        ~TcpServer()
        {
            client?.Close();
        }
    }
}