

using GameOverlay.Drawing;
using GameOverlay.Windows;

namespace Sharp.Drawing
{
    public class BaseOverlay : GraphicsWindow
    {
        public SolidBrush Brush ( System.Drawing.Color color ) { try { return Graphics.CreateSolidBrush ( color.R, color.G, color.B, color.A ); } catch { return null; } }
        public Font Font ( string name, float size, bool bold = false, bool italic = false ) { return new Font ( new SharpDX.DirectWrite.Factory ( ), name, size, bold, italic, true ); }
        public Color GColor ( System.Drawing.Color color ) { return new Color ( color.R, color.G, color.B, color.A ); }

        public void DrawText ( Point pos, string name, IBrush brush, float size = 10, bool bold = false, bool italic = false )
        {
            Graphics.DrawText ( Font ( "consolas", size, bold, italic ), Brush ( System.Drawing.Color.Black ), new Point ( pos.X, pos.Y - 1f ), name ); // Top
            Graphics.DrawText ( Font ( "consolas", size, bold, italic ), Brush ( System.Drawing.Color.Black ), new Point ( pos.X, pos.Y + 1f ), name ); // Bottom
            Graphics.DrawText ( Font ( "consolas", size, bold, italic ), Brush ( System.Drawing.Color.Black ), new Point ( pos.X - 1, pos.Y ), name ); // Left
            Graphics.DrawText ( Font ( "consolas", size, bold, italic ), Brush ( System.Drawing.Color.Black ), new Point ( pos.X + 1, pos.Y ), name ); // Right
            Graphics.DrawText ( Font ( "consolas", size, bold, italic ), brush, new Point ( pos.X, pos.Y ), name );
        }

        public Point CalcStringSize ( string content, bool bold = false, bool italic = false, float size = 10 ) =>
            Graphics.MeasureString ( Font ( "consolas", size, bold, italic ), content );
    }
}
