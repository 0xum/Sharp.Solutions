using System;
using System.Runtime.InteropServices;

namespace Sharp.Imports
{
    public static class Gdi32
    {
        [DllImport ( "Gdi32.dll", EntryPoint = "CreateRoundRectRgn" )]
        public static extern IntPtr CreateRoundRectRgn ( int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse );
    }
}
