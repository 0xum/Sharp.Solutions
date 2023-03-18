using System;
using System.IO;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;
using Veldrid;
using System.Runtime.InteropServices;

using Rectangle = System.Drawing.Rectangle;
using ImGuiNET;
namespace Sharp.ImGUI.Extensions
{
    public class ImGuiImage
    {

        private IntPtr _Handle;
        private Bitmap _Bitmap;
        private Texture _Texture;

        private RgbaByte [ ] _ImageData;

        private ImGuiNET.ImGuiController _ImGuiController ;
        private GraphicsDevice _GraphicsDevice;

        public IntPtr Handle
        {
            get { return _Handle; }
            private set { }
        }

        public Bitmap Bitmap
        {
            get { return _Bitmap; }
        }

        public Texture Texture
        {
            get { return _Texture; }
        }

        public ImGuiImage (ImGuiController imGuiController, GraphicsDevice graphicsDevice )
        {
            _ImGuiController = imGuiController;
            _GraphicsDevice = graphicsDevice;
        }

        public IntPtr LoadFromFile ( string path )
        {
            var data = File.ReadAllBytes(path);

            var stream = new MemoryStream(data);

            _Bitmap = new Bitmap ( stream );

            stream.Dispose ( );

            _ImageData = new RgbaByte [ _Bitmap.Width * _Bitmap.Height ];

            var rect = new Rectangle(0, 0, _Bitmap.Width, _Bitmap.Height);
            var bmpData = _Bitmap.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var stride = bmpData.Stride;
            var scan0 = bmpData.Scan0;

            for ( int y = 0 ; y < _Bitmap.Height ; y++ )
            {
                var rowOffset = y * stride;

                for ( int x = 0 ; x < _Bitmap.Width ; x++ )
                {
                    var pixel = Marshal.ReadInt32(scan0, rowOffset + x * 4);

                    _ImageData [ y * _Bitmap.Width + x ] = new RgbaByte (
                        ( byte ) ( ( pixel >> 16 ) & 0xFF ), // R
                        ( byte ) ( ( pixel >> 8 ) & 0xFF ),  // G
                        ( byte ) ( pixel & 0xFF ),         // B
                        ( byte ) ( ( pixel >> 24 ) & 0xFF )  // A
                    );
                }
            }

            _Bitmap.UnlockBits ( bmpData );

            _Texture = _GraphicsDevice.ResourceFactory.CreateTexture ( TextureDescription.Texture2D ( ( uint ) _Bitmap.Width, ( uint ) _Bitmap.Height, 1, 1, Veldrid.PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Sampled ) );

            _GraphicsDevice.UpdateTexture ( _Texture,
                                                      _ImageData,
                                                      0,
                                                      0,
                                                      0,
                                                      ( uint ) _Bitmap.Width,
                                                      ( uint ) _Bitmap.Height,
                                                      1,
                                                      0,
                                                      0 );

            return _ImGuiController.GetOrCreateImGuiBinding ( _GraphicsDevice.ResourceFactory, _Texture );
        }
    }
}