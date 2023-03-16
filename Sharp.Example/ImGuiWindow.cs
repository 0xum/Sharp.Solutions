
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
    public class ImGuiWindow : SharpWindow
    {
        public ImGuiWindow ( Rectangle size, float framesPerSecond = 60, bool isVisible = true ) : base ( size, framesPerSecond, isVisible )
        {
        }

        public static void CreateImGuiWindow ( Rectangle rect )
        {
            new ImGuiWindow ( rect );
        }

        public override void OnGui ( )
        {
            ImGui.Begin ( "Window" );

            ImGui.Text ( "Hello World!" );

            if ( Input.IsKeyPressed ( VirtualKeys.Return ) )
            {
                ImGui.Text ( "Return pressed." );
            }

            if ( ImGui.Button ( "Click me!" ) )
            {
                ImGui.Text ( "You clicked the button!" );

                Console.WriteLine ( "You clicked the button!" );
            }

            ImGui.End ( );
        }

        public override void OnGuiDraw ( object sender, DrawGraphicsEventArgs e )
        {
            // Keep calling the base method, it will make some necessary updates.
            base.OnGuiDraw ( sender, e );

            BaseOverlay.DrawText ( new Point ( 15, 35 ), $"OnGuiDraw Called [ {Graphics.FPS} Fps ]", BaseOverlay.Brush ( Color.Lime ), 18 );

            if ( Input.IsKeyPressed ( VirtualKeys.F ) )
            {
                var midScreen = new Point(Sdl2Window.Width / 2, Sdl2Window.Height / 2);

                var mPos = Input.GetMousePoint ( );

                var mPoint = new Point(mPos.X - BaseOverlay.X, mPos.Y - BaseOverlay.Y);

                Graphics.DrawLine ( BaseOverlay.Brush ( Color.Cyan ), new Line ( midScreen, mPoint ), 2f );
            }
        }
    }
}