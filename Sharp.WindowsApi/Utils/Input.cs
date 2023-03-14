using System;
using System.Collections.Generic;
using System.Threading;

using Sharp.Enums;
using Sharp.Imports;

namespace Sharp.Utils
{
    /// <summary>
    /// Input Controller.
    /// </summary>
    public static class Input
    {

        private static bool [ ] KeyStates = new bool [ 256 ];

        public static VirtualKeys LastCapturedKey { get; set; }

        private static List<VirtualKeys> VirtualKeysHistory { get; set; }

        static Thread KeyCaptureThread;

        /// <summary>
        /// Creates a keyboard history.
        /// </summary>
        public static void CaptureKeyHistory ( )
        {
            KeyCaptureThread = new Thread ( ( ) =>
             {
                 var keys = Enum.GetValues(typeof(VirtualKeys));

                 VirtualKeysHistory = new List<VirtualKeys> ( );

                 while ( true )
                 {
                     foreach ( VirtualKeys key in keys )
                     {
                         Thread.Sleep ( 1 );

                         if ( IsKeyPressed ( key ) )
                         {
                             VirtualKeysHistory.Add ( key );

                             if ( LastCapturedKey != key )
                             {
                                 LastCapturedKey = key;
                             }
                         }
                     }

                     Thread.Sleep ( 100 );
                 }
             } );

            if ( KeyCaptureThread != null && KeyCaptureThread.ThreadState != ThreadState.Running )
            {
                KeyCaptureThread.Start ( );
            }
        }

        /// <summary>
        /// Returns the last pressed key.
        /// </summary>
        /// <returns></returns>
        public static VirtualKeys GetLastPressedKey ( )
        {
            var lastKey = VirtualKeys.Noname;

            var keys = Enum.GetValues(typeof(VirtualKeys));

            while ( lastKey is VirtualKeys.Noname )
            {
                foreach ( VirtualKeys key in keys )
                {
                    if ( IsKeyPressed ( key ) )
                    {
                        lastKey = key;
                    }
                }

                Thread.Sleep ( 50 );
            }

            return lastKey;
        }

        /// <summary>
        /// Check if the key is down.
        /// </summary>
        /// <param name="vKey"></param>
        /// <returns></returns>
        public static bool IsKeyDown ( VirtualKeys vKey )
        {
            if ( ( ( User32.GetAsyncKeyState ( vKey ) & 0x8000 ) != 0 ) != KeyStates [ ( int ) vKey ] )
            {
                return KeyStates [ ( int ) vKey ] = !KeyStates [ ( int ) vKey ];
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the way was pressed.
        /// </summary>
        /// <param name="vKey"></param>
        /// <returns></returns>
        public static bool IsKeyPressed ( VirtualKeys vKey )
        {
            return ( User32.GetAsyncKeyState ( vKey ) & 0x8000 ) != 0;
        }

        /// <summary>
        /// Simulate a key event.
        /// </summary>
        /// <param name="vKey"></param>
        public static void SendKey ( VirtualKeys vKey )
        {
            User32.keybd_event ( ( byte ) vKey, 0, 0, 0 );
        }

        /// <summary>
        /// Set cursor to a position on screen.
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        public static void SetCursorPosition ( int x, int y )
        {
            User32.SetCursorPos ( x, y );
        }

        /// <summary>
        /// Simulate a mouse button click.
        /// </summary>
        /// <param name="button"></param>
        public static void SendMouseClick ( MouseButtons button = MouseButtons.Left )
        {
            User32.GetCursorPos ( out var pos );

            switch ( button )
            {
                case MouseButtons.Left:

                    User32.mouse_event ( Enums.Flags.MouseEventFlags.LeftDown, pos.X, pos.Y, 0, 0 );
                    User32.mouse_event ( Enums.Flags.MouseEventFlags.LeftUp, pos.X, pos.Y, 0, 0 );

                    break;
                case MouseButtons.Right:

                    User32.mouse_event ( Enums.Flags.MouseEventFlags.RightDown, pos.X, pos.Y, 0, 0 );
                    User32.mouse_event ( Enums.Flags.MouseEventFlags.RightUp, pos.X, pos.Y, 0, 0 );

                    break;
                case MouseButtons.Middle:

                    User32.mouse_event ( Enums.Flags.MouseEventFlags.MiddleDown, pos.X, pos.Y, 0, 0 );
                    User32.mouse_event ( Enums.Flags.MouseEventFlags.MiddleUp, pos.X, pos.Y, 0, 0 );

                    break;
            }
        }

        /// <summary>
        /// Simulate a mouse button press.
        /// </summary>  
        /// <param name="button">Mouse Button</param>
        /// <param name="Ms">How long the button will be pressed, If is zero the button will be pressed indefinitely</param>
        public static void SendMousePress ( MouseButtons button = MouseButtons.Left, int Ms = 100 )
        {
            User32.GetCursorPos ( out var pos );

            switch ( button )
            {
                case MouseButtons.Left:

                    User32.mouse_event ( Enums.Flags.MouseEventFlags.LeftDown, pos.X, pos.Y, 0, 0 );

                    if ( Ms > 0 )
                    {
                        Thread.Sleep ( Ms );
                        User32.mouse_event ( Enums.Flags.MouseEventFlags.LeftUp, pos.X, pos.Y, 0, 0 );
                    }

                    break;
                case MouseButtons.Right:

                    User32.mouse_event ( Enums.Flags.MouseEventFlags.RightDown, pos.X, pos.Y, 0, 0 );

                    if ( Ms > 0 )
                    {
                        Thread.Sleep ( Ms );
                        User32.mouse_event ( Enums.Flags.MouseEventFlags.RightUp, pos.X, pos.Y, 0, 0 );
                    }

                    break;
                case MouseButtons.Middle:

                    User32.mouse_event ( Enums.Flags.MouseEventFlags.MiddleDown, pos.X, pos.Y, 0, 0 );

                    if ( Ms > 0 )
                    {
                        Thread.Sleep ( Ms );
                        User32.mouse_event ( Enums.Flags.MouseEventFlags.MiddleUp, pos.X, pos.Y, 0, 0 );
                    }

                    break;
            }
        }
    }
}
