using System.Drawing;
using System.Runtime.InteropServices;

namespace Sharp.Structs
{
    [StructLayout ( LayoutKind.Sequential )]
    public struct MousePoint
    {
        public int X;
        public int Y;

        public MousePoint ( int x, int y )
        {
            X = x;
            Y = y;
        }

        public static implicit operator Point ( MousePoint point )
        {
            return new Point ( point.X, point.Y );
        }
    }
}
