using Sharp.Structs;

using Sharp.Enums;
using Sharp.Enums.Flags;

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Sharp.Imports
{
    public static class User32
    {

        #region Mouse or Keyboard Input Related
        
        [DllImport ( "User32.dll" )]
        public static extern short GetAsyncKeyState ( VirtualKeys vKey );

        [DllImport ( "user32.dll" )]
        public static extern void keybd_event ( byte bVk, byte bScan, uint dwFlags, int dwExtraInfo );

        [DllImport ( "user32.dll" )]
        public static extern void mouse_event ( MouseEventFlags dwFlags, int dx, int dy, int dwData, int dwExtraInfo );

        [DllImport ( "user32.dll", EntryPoint = "SetCursorPos" )]
        [return: MarshalAs ( UnmanagedType.Bool )]
        public static extern bool SetCursorPos ( int x, int y );

        [DllImport ( "user32.dll" )]
        [return: MarshalAs ( UnmanagedType.Bool )]
        public static extern bool GetCursorPos ( out MousePoint lpMousePoint );

        #endregion

        #region Windows Managment Related

        [DllImport ( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
        public static extern int GetWindowThreadProcessId ( IntPtr handle, out int processId );

        [DllImport ( "user32.dll", SetLastError = true )]
        public static extern bool ClientToScreen ( IntPtr hWnd, out Point lpPoint );

        [DllImport ( "user32.dll", SetLastError = true )]
        public static extern bool GetClientRect ( IntPtr hWnd, out Rectangle lpRect );

        [DllImport ( "user32.dll", SetLastError = true )]
        public static extern int GetWindowLong ( IntPtr hWnd, int nIndex );

        [DllImport ( "user32.dll" )]
        public static extern bool ShowWindow ( IntPtr hWnd, int nCmdShow );

        [DllImport ( "user32.dll" )]
        public static extern int SetWindowLong ( IntPtr hWnd, WindowLongParamFlags nIndex, IntPtr dwNewLong );

        [DllImport ( "user32.dll" )]
        [return: MarshalAs ( UnmanagedType.Bool )]
        public static extern bool SetForegroundWindow ( IntPtr hWnd );

        [DllImport ( "user32.dll" )]
        public static extern IntPtr GetForegroundWindow ( );

        [DllImport ( "user32.dll", SetLastError = true )]
        public static extern bool SetLayeredWindowAttributes ( IntPtr hWnd, uint crKey, byte bAlpha, LayeredWindowFlags dwFlags );

        #endregion

    }
}
