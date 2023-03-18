
using ImGuiNET;
using Sharp.ImGUI;
using System.Diagnostics;

using Sharp.Utils;
using Sharp.Enums;

using GameOverlay.Windows;
using GameOverlay.Drawing;

using Color = System.Drawing.Color;

namespace Sharp.Example
{
    public class ImGuiOverlay : SharpGui
    {

        public ImGuiOverlay ( Process process, float framesPerSecond = 60, bool isVisible = true ) : base ( process, framesPerSecond, isVisible )
        {
        }

        public override void OnGuiVisible ( )
        {
            ImGui.ShowDemoWindow ( );
        }

        public override void OnGuiDraw ( object sender, DrawGraphicsEventArgs e )
        {
            base.OnGuiDraw ( sender, e );

            BaseOverlay.DrawText ( new Point ( 15, 35 ), $"OnGuiDraw Called [ {Graphics.FPS} Fps ]", BaseOverlay.Brush ( Color.Lime ), 18 );
        }
    }
}