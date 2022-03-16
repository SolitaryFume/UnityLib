using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityLib.Net;
using Proto;
using System;

public class PlayerInput:MonoBehaviour
{
    public UdpServer udpServer;
    [SerializeField] private string ip = "127.0.0.1";
    [SerializeField] private int port = 8888;

    [SerializeField] private int m_fps = 15;
    private int frame;

    private void Awake()
    {
      
    }

    private CancellationTokenSource source;
    private async void Start()
    {
        if (PlayerManager.Instance.playerType == PlayerManager.PlayerType.Net)
        {
            udpServer = new UdpServer();
            udpServer.Connect(ip, port);
            udpServer.onReceiveMessage += NetManagerHandle.MessageHandle;
            udpServer.serialize += Proto.ProtoHelp.Encoder;
            udpServer.deserialize += Proto.ProtoHelp.Decoder;
            udpServer.StartReceive();
            udpServer.SendMessage(new JoinRoomRequest() { });
            Debug.Log("udpServer start succeed !");
        }

        
    }

    public async void StartSync()
    {
        if (source != null && !source.IsCancellationRequested)
            return;

        var tickTime = Mathf.RoundToInt(1000f / m_fps);
        source = new CancellationTokenSource();
        while (!source.IsCancellationRequested)
        {
            await Task.Delay(tickTime, source.Token);
            frame++;
            InputTick();
        }
    }

    public void StopSync()
    {
        source?.Cancel();
    }

    private void OnDestroy()
    {
        source.Cancel();
    }

    private static Vector2 startDirection = new Vector2(0,1);
    private void InputTick()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        var direction = 0;
        if (h != 0 || v != 0)
        { 
            var dir = new Vector2(h, v);
            direction = Mathf.RoundToInt(Vector2.SignedAngle(dir, startDirection));
            direction = direction >= 0 ? direction+1 : 361 + direction;
        }

        var frameInput = new FrameInfo()
        {
            tick = frame,
            direction = (ushort)direction,
        };
        ReadOnlySpan<byte> d = Proto.ProtoHelp.Encoder(frameInput);
        var mg = Proto.ProtoHelp.Decoder(d.Slice(4));
        switch (PlayerManager.Instance.playerType)
        {
            case PlayerManager.PlayerType.Single:
                PlayerManager.Instance.Execute(new FrameInfo[1] { frameInput });
                break;
            case PlayerManager.PlayerType.Net:
                udpServer.SendMessage(frameInput);
                PlayerManager.Instance.Execute(new FrameInfo[1] { frameInput });
                break;
            case PlayerManager.PlayerType.Playback:
                PlayerManager.Instance.Execute(new FrameInfo[1] { frameInput });
                break;
            default:
                break;
        }
    }
}
