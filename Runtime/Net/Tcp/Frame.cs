using UnityEngine;
using ProtoBuf;
using Proto;

[System.Serializable]
[ProtoContract]
public class Frame : IMessage
{
    [ProtoMember(1)]
    public uint index { get; set; }
    [ProtoMember(2)]
    public ushort direction { get; set; }
}