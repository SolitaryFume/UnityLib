using System.IO;
using Proto;

public static class MessageBinary
{
    public static void Write(this BinaryWriter writer, FrameInfo frame)
    {
        writer.Write(frame.tick);
        writer.Write(frame.direction);
    }

    public static FrameInfo ReadFrame(this BinaryReader reader)
    {
        var frame = new FrameInfo();
        frame.tick = reader.ReadInt32();
        frame.direction = reader.ReadUInt16();
        return frame;
    }
}