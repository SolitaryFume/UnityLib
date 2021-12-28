using System;

[Flags]
public enum WidgetStatus : byte
{
        Display = 0x01,
        Active = 0x10,
}
