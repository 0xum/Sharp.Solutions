
using ImGuiNET;
using Sharp.ImGUI;
using System.Diagnostics;

using Sharp.Utils;
using Sharp.Enums;

using GameOverlay.Windows;
using GameOverlay.Drawing;

using Color = System.Drawing.Color;

#pragma warning disable CS8601

namespace Sharp.Example
{
    public class Program : SharpGui
    {

        // Name should be without .EXE
        private static string _TargetProcessName = "notepad";

        public Program ( Process process, float framesPerSecond = 60, bool isVisible = true ) : base ( process, framesPerSecond, isVisible )
        {
        }

        private static void Main ( string [ ] args )
        {
            var process = Process.GetProcessesByName ( _TargetProcessName ).FirstOrDefault ( );

            if ( process is not null )
            {
                Console.WriteLine ( "Process found. Loading ImGui" );

                var program = new Program ( process );

                while ( !process.HasExited && !program.HasExited )
                    continue;

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
        }
    }
}