using System;
using System.Runtime.InteropServices;

using Sharp.Structs;

namespace Sharp.Imports
{
    public static class DwmApi
    {

        [DllImport ( "dwmapi.dll" )]
        public static extern IntPtr DwmExtendFrameIntoClientArea ( IntPtr hWnd, ref Margins pMarInset );

    }
}
