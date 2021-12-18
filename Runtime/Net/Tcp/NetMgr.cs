// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using System.IO;
// using System.Threading;
// using static UnityLib.Net.TcpProxy_Ex;

// #if !ProtobufNet
// using ProtoBuf;

// public interface INetMsg
// {

// }

// [ProtoContract]
// public class Login
// {
//     [ProtoMember(1)]
//     public string Id { get; set; }
// }

// #endif


// namespace UnityLib.Net
// {
//     public sealed class NetMgr : MonoBehaviour
//     {
//         private List<TcpProxy> tcpProxies = new List<TcpProxy>();
//         public List<RPCAwaiter> netMsgs = new List<RPCAwaiter>();

//         private static NetMgr _instance;
//         public static NetMgr Instance
//         {
//             get
//             {
//                 if (_instance == null)
//                 {
//                     var obj = new GameObject("[NetMgr]");
//                     DontDestroyOnLoad(obj);
//                     _instance = obj.AddComponent<NetMgr>();
//                 }
//                 return _instance;
//             }
//         }

//         public TcpProxy CreateTcp(string host, int port)
//         {

//             //File.WriteAllText(@"H:\log.txt",Thread.CurrentThread.ManagedThreadId.ToString());
//             //Debug.Log(Thread.CurrentThread.ManagedThreadId);
//             if (string.IsNullOrEmpty(host))
//                 throw new ArgumentNullException("host");
//             var tcp = new TcpProxy();
//             tcp.Connect(host, port);
//             tcp.Receive();
//             tcpProxies.Add(tcp);
//             return tcp;
//         }

//         public void DeleteTcp(TcpProxy tcp)
//         {
//             if (tcp == null)
//                 return;
//             tcp.Close();
//             tcp.Dispose();
//             tcpProxies.Remove(tcp);
//         }

//         private void Update()
//         {
//             for (int i = tcpProxies.Count - 1; i >= 0; i--)
//             {
//                 var tcp = tcpProxies[i];
//                 while (tcp.TryDequeue(out var msgData))
//                 {
//                     Debug.Log(msgData.Length);
//                     if (netMsgs.Count > 0)
//                     {
//                         var msgaw = netMsgs[i];

//                         msgaw.SetResult(msgData);
//                         msgaw.IsCompleted = true;
//                     }
//                 }
//             }
//         }

//         private void OnDestroy()
//         {
//             foreach (var tcp in tcpProxies)
//             {
//                 tcp.Close();
//                 tcp.Dispose();
//             }
//         }
//     }
// }