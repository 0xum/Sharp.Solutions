
using ImGuiNET;
using Sharp.ImGUI;

using GameOverlay.Windows;
using GameOverlay.Drawing;

using Rectangle = System.Drawing.Rectangle;

namespace Sharp.Example
{
    public class ImGuiWindow : SharpWindow
    {
        public ImGuiWindow ( Rectangle size, float framesPerSecond = 60, bool isVisible = true ) : base ( size, framesPerSecond, isVisible )
        {
        }

        public override void OnGui ( )
        {
            ImGui.ShowDemoWindow ( );
        }

        public override void OnGuiDraw ( object sender, DrawGraphicsEventArgs e )
        {
            // Keep calling the base method, it will make some necessary updates.
            base.OnGuiDraw ( sender, e );

            BaseOverlay.DrawText ( new Point ( 15, 35 ), $"OnGuiDraw Called [ {Graphics.FPS} Fps ]", BaseOverlay.Brush ( Color.Green ), 18 );
        }
    }
}