using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

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
            var buffer = new byte[sizeof(int)*2 + msg.m_data.Length];
            Array.Copy(BitConverter.GetBytes(buffer.Length), buffer,sizeof(int));
            Array.Copy(BitConverter.GetBytes(msg.m_id), 0, buffer, sizeof(int), sizeof(int));
            Array.Copy(msg.m_data, 0, buffer, sizeof(int) * 2, msg.Data.Length);
            return msg;
        }
    }

    public class TcpProxy : TcpClient
    {
        private int a;
        private void Initialize() { 
            ushort b = 20;
            a = b;
        }

        public TcpProxy():base()
        {
            Initialize();
            
        }
        public TcpProxy(IPEndPoint localEP):base(localEP)
        {
            Initialize();
        }
        public TcpProxy(AddressFamily family):base(family)
        {
            Initialize();
        }

        public TcpProxy(string hostname, int port):base(hostname, port)
        {
            Initialize();
        }

        public async void Send(NetMsg msg)
        {
            byte[] buffer = msg;
            //await Client.BeginSend(buffer, 0, buffer.Length,SocketFlags.None,SnedCallBack,null);
        }

        public void Receive()
        {
            //Client.Receive();
        }
    }
}