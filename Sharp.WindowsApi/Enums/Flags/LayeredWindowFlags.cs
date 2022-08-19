using System;

namespace Sharp.Enums.Flags
{
    [Flags]
    public enum LayeredWindowFlags : uint
    {
        LWA_ALPHA = 0x00000002,
        LWA_COLORKEY = 0x00000001,
    }
}
