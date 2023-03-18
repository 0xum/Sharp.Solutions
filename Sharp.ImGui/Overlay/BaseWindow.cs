

using GameOverlay.Drawing;
using GameOverlay.Windows;
using Vulkan.Xlib;

namespace Sharp.ImGUI.Overlay
{
    public class BaseOverlay : GraphicsWindow
    {
        public IntPtr WindowHandle;

        public BaseOverlay ( IntPtr handle, int fps = 30 )
        {
            Create ( );

            FPS = fps;

            IsTopmost = true;
            IsVisible = true;
            Graphics.MeasureFPS = true;
            Graphics.PerPrimitiveAntiAliasing = true;
            Graphics.TextAntiAliasing = true;
            Graphics.UseMultiThreadedFactories = true;

            WindowHandle = handle;

            FitTo ( WindowHandle );

            Show ( );
        }

        public SolidBrush Brush ( System.Drawing.Color color )
        {
            try
            {
                return Graphics.CreateSolidBrush ( color.R, color.G, color.B, color.A );
            }
            catch
            {
                return default;
            }
        }

        public SolidBrush Brush ( GameOverlay.Drawing.Color color )
        {
            try
            {
                return Graphics.CreateSolidBrush ( color.R, color.G, color.B, color.A );
            }
            catch
            {
                return default;
            }
        }

        public Font Font ( string name, float size, bool bold = false, bool italic = false )
        {
            return new Font ( new SharpDX.DirectWrite.Factory ( ), name, size, bold, italic, true );
        }

        public Color GColor ( System.Drawing.Color color )
        {
            return new Color ( color.R, color.G, color.B, color.A );
        }

        public void DrawText ( Point pos, string content, IBrush brush, float size = 10, bool bold = false, bool italic = false )
        {
            Graphics.DrawText ( Font ( "consolas", size, bold, italic ), Brush ( System.Drawing.Color.Black ), new Point ( pos.X, pos.Y - 1f ), content ); // Top
            Graphics.DrawText ( Font ( "consolas", size, bold, italic ), Brush ( System.Drawing.Color.Black ), new Point ( pos.X, pos.Y + 1f ), content ); // Bottom
            Graphics.DrawText ( Font ( "consolas", size, bold, italic ), Brush ( System.Drawing.Color.Black ), new Point ( pos.X - 1, pos.Y ), content ); // Left
            Graphics.DrawText ( Font ( "consolas", size, bold, italic ), Brush ( System.Drawing.Color.Black ), new Point ( pos.X + 1, pos.Y ), content ); // Right
            Graphics.DrawText ( Font ( "consolas", size, bold, italic ), brush, new Point ( pos.X, pos.Y ), content );
        }

        public Point CalcStringSize ( string content, bool bold = false, bool italic = false, float size = 10 )
        {
            return Graphics.MeasureString ( Font ( "consolas", size, bold, italic ), content );
        }

    }
}
