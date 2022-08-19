
using System.Drawing;
using System.Runtime.InteropServices;

namespace Sharp.Structs
{
    [StructLayout ( LayoutKind.Sequential )]
    public struct Margins
    {
        private int left, right, top, bottom;

        public static Margins FromRectangle ( Rectangle rectangle )
        {
            var margins = new Margins
            {
                left = rectangle.Left,
                right = rectangle.Right,
                top = rectangle.Top,
                bottom = rectangle.Bottom
            };
            return margins;
        }
    }
}
