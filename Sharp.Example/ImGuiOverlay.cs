
using ImGuiNET;
using Sharp.ImGUI;
using System.Diagnostics;

using Sharp.Utils;
using Sharp.Enums;

using GameOverlay.Windows;
using GameOverlay.Drawing;

using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;

using System;
using System.Numerics;

namespace Sharp.Example
{
    public class ImGuiOverlay : SharpGui
    {

        public ImGuiOverlay ( Process process, float framesPerSecond = 60, bool isVisible = true ) : base ( process, framesPerSecond, isVisible )
        {
        }

        public static void CrateImGuiOverlay ( string targetProcessName)
        {
            var process = Process.GetProcessesByName ( targetProcessName ).FirstOrDefault ( );

            if ( process is not null )
            {
                Console.WriteLine ( "Process found. Loading ImGui" );

                var overlay = new ImGuiOverlay ( process );

                Console.WriteLine ( "Process or program has exited. Closing." );
            }
            else
            {
                Console.WriteLine ( "Process not found." );
            }
        }

        public override void OnGuiVisible ( )
        {
            ImGui.Begin ( TargetProcess.MainWindowTitle );

            ImGui.Text ( "Hello World!" );

            if ( Input.IsKeyPressed ( VirtualKeys.Return ) )
            {
                ImGui.Text ( "Return pressed." );
            }

            ImGui.End ( );
        }

        public override void OnGuiDraw ( object sender, DrawGraphicsEventArgs e )
        {
            base.OnGuiDraw ( sender, e );

            if ( !IsVisible )
            {
                BaseOverlay.DrawText ( new Point ( 5, 80 ), "Press Insert to Show/Hide IMGUI", BaseOverlay.Brush ( Color.Lime ), 18 );
                BaseOverlay.DrawText ( new Point ( 5, 100 ), "Press END to Close Program", BaseOverlay.Brush ( Color.Red ), 18 );
            }

            var midScreen = new Point(Sdl2Window.Width / 2, Sdl2Window.Height / 2);

            var mPos = Input.GetMousePoint ( );

            var mPoint = new Point(mPos.X - BaseOverlay.X, mPos.Y - BaseOverlay.Y);

            Graphics.DrawLine ( BaseOverlay.Brush ( Color.Cyan ), new Line ( midScreen, mPoint ), 2f );

        }
    }
}