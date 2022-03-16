using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using Proto;
using UnityLib.Net;

public partial class PlayerManager : MonoBehaviour
{
    public enum PlayerType : byte
    {
        Single = 0,
        Net,
        Playback
    }

    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;

        if (playerType == PlayerType.Playback)
        {
            operationStream = File.OpenRead(operationFile);
            binaryReader = new BinaryReader(operationStream);
        }
        else
        {
            operationStream = File.Open(operationFile, FileMode.Create);
            binaryWriiter = new BinaryWriter(operationStream);
        }

        RegisterMessageHanle();
    }

    private void InitData()
    {
        predictFrame = new Stack<FrameInfo[]>();
    }

    private void OnDestroy()
    {
        binaryReader?.Close();
        binaryWriiter?.Close();
        operationStream?.Close();

        UnRegisterMessageHanle();
    }

    [SerializeField] private PlayerType m_playerType;
    [SerializeField] private string operationFile = @"H:\ECS\Assets\playoperation.bin";

    private FileStream operationStream;
    private BinaryReader binaryReader;
    private BinaryWriter binaryWriiter;
    private FrameInfo[] lastframes;

    private Stack<FrameInfo[]> predictFrame;

    public PlayerType playerType => m_playerType;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerProxy[] proxies;
    public void Execute(FrameInfo[] frames)
    {
        if (playerType == PlayerType.Playback)
        {
            frames[0] = binaryReader.ReadFrame();
        }
        else
        {
            binaryWriiter.Write(frames[0]);
        }

        lastframes = frames;
        for (int i = 0; i < frames.Length; i++)
        {
            proxies[i].Execute(frames[i]);
        }
    }

    /// <summary>
    /// 预测
    /// </summary>
    private void Predict()
    { 
        
    }

    private void Revert()
    {
        while (predictFrame.Count>0)
        {
            var frames = predictFrame.Pop();
            for (int i = frames.Length - 1; i >= 0; i--)
            {
                proxies[i].Reset(frames[i]);
            }
        }
    }
}

public partial class PlayerManager
{
    private IDisposable listening_FramePack;
    private IDisposable listening_StartRoomSync;

    public void RegisterMessageHanle()
    {
        listening_FramePack = NetManagerHandle.Register<FramePackage>(FramePackHandle);
        listening_StartRoomSync = NetManagerHandle.Register<StartRoomSync>(StartRoomSyncHandle);
    }

    private void UnRegisterMessageHanle()
    {
        listening_FramePack.Dispose();
    }

    public void FramePackHandle(FramePackage package)
    {
        Debug.Log(package.tick);
    }

    public void StartRoomSyncHandle(StartRoomSync sync)
    {
        playerInput.StartSync();
    }
}
