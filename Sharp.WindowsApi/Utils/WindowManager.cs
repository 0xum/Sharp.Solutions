using System;
using System.Linq;
using System.Drawing;
using System.Diagnostics;

using Sharp.Imports;
using Sharp.Structs;
using Sharp.Enums;
using Sharp.Enums.Flags;

namespace Sharp.Utils
{
    public static class WindowManager
    {

        public static void HideConsole ( )
        {
            User32.ShowWindow ( Kernel32.GetConsoleWindow ( ), 0 );
        }
        public static void ShowConsole ( )
        {
            User32.ShowWindow ( Kernel32.GetConsoleWindow ( ), 5 );
        }
        public static void AllocateConsole ( )
        {
            Kernel32.AllocConsole ( );
        }
        public static void FreeConsole ( )
        {
            Kernel32.FreeConsole ( );
        }

        public static void SetWindowAsForeground ( string processName )
        {

            var process = Process.GetProcessesByName ( processName ).FirstOrDefault ( );

            if ( process != null )
            {
                User32.SetForegroundWindow ( process.MainWindowHandle );
            }
        }
        public static void SetWindowAsForeground ( IntPtr mainHwnd ) => User32.SetForegroundWindow ( mainHwnd );

        public static void EnableTransparency ( IntPtr handle, Rectangle size )
        {
            User32.SetLayeredWindowAttributes ( handle, 0, 255, LayeredWindowFlags.LWA_ALPHA );
            Margins margins = Margins.FromRectangle ( size );
            DwmApi.DwmExtendFrameIntoClientArea ( handle, ref margins );
        }
        public static void ManageClickThrough ( IntPtr handle, bool state, bool stealthMode = true )
        {

            if ( state )
            {
                if ( stealthMode )
                {

                    int windowLong = User32.GetWindowLong ( handle, ( int ) WindowLongParamFlags.GWL_EXSTYLE ) | ( int ) WindowStylesExFlags.WS_EX_LAYERED | ( int ) WindowStylesExFlags.WS_EX_TRANSPARENT | ( int ) WindowStylesExFlags.WS_EX_TOOLWINDOW;
                    User32.SetWindowLong ( handle, WindowLongParamFlags.GWL_EXSTYLE, new IntPtr ( windowLong ) );

                }
                else
                {

                    int windowLong = User32.GetWindowLong ( handle, ( int ) WindowLongParamFlags.GWL_EXSTYLE ) & ( int ) WindowStylesExFlags.WS_EX_LAYERED & ( int ) WindowStylesExFlags.WS_EX_TRANSPARENT;
                    User32.SetWindowLong ( handle, WindowLongParamFlags.GWL_EXSTYLE, new IntPtr ( windowLong ) );

                }

            }
            else
            {
                if ( stealthMode )
                {

                    int windowLong = User32.GetWindowLong ( handle, ( int ) WindowLongParamFlags.GWL_EXSTYLE ) & ( int ) WindowStylesExFlags.WS_EX_LAYERED & ( int ) WindowStylesExFlags.WS_EX_TRANSPARENT | ( int ) WindowStylesExFlags.WS_EX_TOOLWINDOW;
                    User32.SetWindowLong ( handle, WindowLongParamFlags.GWL_EXSTYLE, new IntPtr ( windowLong ) );

                }
                else
                {

                    int windowLong = User32.GetWindowLong ( handle, ( int ) WindowLongParamFlags.GWL_EXSTYLE ) & ( int ) WindowStylesExFlags.WS_EX_LAYERED & ( int ) WindowStylesExFlags.WS_EX_TRANSPARENT;
                    User32.SetWindowLong ( handle, WindowLongParamFlags.GWL_EXSTYLE, new IntPtr ( windowLong ) );

                }
            }
        }

        public static bool ApplicationIsActivated ( string name )
        {
            var activatedHandle = User32.GetForegroundWindow ( );

            if ( activatedHandle == IntPtr.Zero ) return false;

            User32.GetWindowThreadProcessId ( activatedHandle, out var activeProcId );

            var target = Process.GetProcessesByName ( name ).FirstOrDefault ( );

            if ( target is null )
                return false;

            return activeProcId == target.Id;
        }
        public static bool ApplicationIsActivated ( int id )
        {
            var activatedHandle = User32.GetForegroundWindow ( );

            if ( activatedHandle == IntPtr.Zero ) return false;

            User32.GetWindowThreadProcessId ( activatedHandle, out var activeProcId );

            var target = Process.GetProcessById ( id );

            if ( target is null )
                return false;

            return activeProcId == target.Id;
        }

        public static bool CurrentApplicationIsActivated ( )
        {
            var activatedHandle = User32.GetForegroundWindow ( );

            if ( activatedHandle == IntPtr.Zero ) return false;

            User32.GetWindowThreadProcessId ( activatedHandle, out var activeProcId );

            return activeProcId == Process.GetCurrentProcess ( ).Id;
        }
        public static void SetCurrentWindowAsForeground ( )
        {
            User32.SetForegroundWindow ( Process.GetCurrentProcess ( ).MainWindowHandle );
        }

        public static Rectangle GetWindowSize ( string process )
        {
            var handle = IntPtr.Zero;

            var proc = Process.GetProcessesByName ( process ).FirstOrDefault ( );

            if ( proc != null )
                handle = proc.MainWindowHandle;

            return GetClientRectangle ( handle );
        }
        public static Rectangle GetWindowSize ( IntPtr handle )
        {
            return GetClientRectangle ( handle );
        }

        private static Rectangle GetClientRectangle ( IntPtr handle )
        {
            return User32.ClientToScreen ( handle, out var point ) && User32.GetClientRect ( handle, out var rect )
                ? new Rectangle ( point.X, point.Y, rect.Right - rect.Left, rect.Bottom - rect.Top )
                : default;
        }

    }
}
