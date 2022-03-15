using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
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

    }

    private void InitData()
    {
        predictFrame = new Stack<Frame[]>();
    }

    private void OnDestroy()
    {
        binaryReader?.Close();
        binaryWriiter?.Close();
        operationStream?.Close();
    }

    [SerializeField] private PlayerType m_playerType;
    [SerializeField] private string operationFile = @"H:\ECS\Assets\playoperation.bin";

    private FileStream operationStream;
    private BinaryReader binaryReader;
    private BinaryWriter binaryWriiter;
    private Frame[] lastframes;

    private Stack<Frame[]> predictFrame;

    public PlayerType playerType => m_playerType;

    [SerializeField] private PlayerProxy[] proxies;
    public void Execute(Frame[] frames)
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


public static class MessageBinary
{
    public static void Write(this BinaryWriter writer, Frame frame)
    {
        writer.Write(frame.index);
        writer.Write(frame.direction);
    }

    public static Frame ReadFrame(this BinaryReader reader)
    {
        var frame = new Frame();
        frame.index = reader.ReadUInt32();
        frame.direction = reader.ReadUInt16();
        return frame;
    }
}