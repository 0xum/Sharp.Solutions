using System;
using System.Linq;
using System.Drawing;
using System.Numerics;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

using ImGuiNET;
using ImPlotNET;
using ImGui = ImGuiNET.ImGui;

using Sharp.Utils;
using Sharp.Enums;

using Sharp.ImGUI.Overlay;
using Rectangle = System.Drawing.Rectangle;

#pragma warning disable CS8618

namespace Sharp.ImGUI
{
    public class SharpWindow
    {

        public static SharpWindow Instance;

        /// <summary>
        /// Called when gui is visible.
        /// </summary>
        public virtual void OnGui ( ) { }

        /// <summary>
        /// Called on every updated frame from graphics.
        /// </summary>
        public virtual void OnGuiDraw ( object sender, GameOverlay.Windows.DrawGraphicsEventArgs e )
        {
            Graphics.ClearScene ( BaseOverlay.Brush ( Color.Transparent ) );
            BaseOverlay.FitTo ( Sdl2Window.Handle );
            BaseOverlay.IsTopmost = WindowManager.CurrentApplicationIsActivated ( );
        }

        public SharpWindow ( Rectangle size, float framesPerSecond = 60f, bool isVisible = true )
        {
            // Find antoher way to get the screen bounds.
            WindowSize = size;
            Instance = this;

            CreateWindow ( framesPerSecond );
        }

        public bool IsVisible;
        public bool HasExited;

        public Rectangle WindowSize;
        public Sdl2Window Sdl2Window;
        public GraphicsDevice GraphicsDevice;
        public CommandList CommandList;
        public ImGuiController ImGuiController;
        public Process TargetProcess => Process.GetCurrentProcess ( );
        public VirtualKeys ToggleVisibilityKey = VirtualKeys.Insert, EscapeKey = VirtualKeys.End;
        public Vector4 BackgroundColor = new Vector4(0.0f, 0.00f, 0.00f, 0.55f);
        public Vector4 ClearColor = new Vector4(0f, 0f, 0f, 0f);
        public BaseOverlay BaseOverlay;
        public GameOverlay.Drawing.Graphics Graphics => BaseOverlay.Graphics;

        void HandleReResize ( )
        {
            try
            {
                GraphicsDevice.MainSwapchain.Resize ( ( uint ) Sdl2Window.Width, ( uint ) Sdl2Window.Height );

                ImGuiController.WindowResized ( Sdl2Window.Width, Sdl2Window.Height );
            }
            catch
            {
            }
        }

        public void CreateWindow ( float framesPerSecond )
        {

            var graphicsDeviceOptions = new GraphicsDeviceOptions(true, null, true);

            Sdl2Window = new Sdl2Window ( "ImGui Window", WindowSize.X, WindowSize.Y, WindowSize.Width, WindowSize.Height, SDL_WindowFlags.Resizable | SDL_WindowFlags.Shown, true );

            BaseOverlay = new BaseOverlay ( Sdl2Window.Handle, ( int ) framesPerSecond );
            BaseOverlay.DrawGraphics += OnGuiDraw;
            Graphics.MeasureFPS = true;

            GraphicsDevice = VeldridStartup.CreateGraphicsDevice ( Sdl2Window, graphicsDeviceOptions );

            Sdl2Window.Resized += HandleReResize;

            CommandList = GraphicsDevice.ResourceFactory.CreateCommandList ( );

            ImGuiController = new ImGuiController ( GraphicsDevice, GraphicsDevice.MainSwapchain.Framebuffer.OutputDescription, Sdl2Window.Width, Sdl2Window.Height );

            while ( Sdl2Window.Exists )
            {
                //WindowManager.EnableTransparency ( Sdl2Window.Handle, WindowSize );

                if ( !Sdl2Window.Exists )
                    break;

                if ( TargetProcess.HasExited )
                    break;

                ImGuiController.Update ( 1f / framesPerSecond, Sdl2Window.PumpEvents ( ) );

                CommandList.Begin ( );

                try
                {
                    CommandList.SetFramebuffer ( GraphicsDevice.MainSwapchain.Framebuffer );
                }
                catch
                {
                }

                OnGui ( );

                try
                {
                    if ( WindowManager.CurrentApplicationIsActivated ( ) || WindowManager.ApplicationIsActivated ( TargetProcess.Id ) )
                    {

                        if ( IsVisible )
                        {
                            CommandList.ClearColorTarget ( 0, new RgbaFloat ( BackgroundColor.X, BackgroundColor.Y, BackgroundColor.Z, BackgroundColor.W ) );
                        }
                        else
                        {
                            CommandList.ClearColorTarget ( 0, new RgbaFloat ( ClearColor.X, ClearColor.Y, ClearColor.Z, ClearColor.W ) );
                        }
                    }
                    else
                    {
                        CommandList.ClearColorTarget ( 0, new RgbaFloat ( ClearColor.X, ClearColor.Y, ClearColor.Z, ClearColor.W ) );
                    }
                }
                catch
                {
                }

                ImGuiController.Render ( GraphicsDevice, CommandList );

                CommandList.End ( );

                GraphicsDevice.SubmitCommands ( CommandList );
                GraphicsDevice.SwapBuffers ( GraphicsDevice.MainSwapchain );
                Thread.Sleep ( 50 );
            }

            GraphicsDevice.WaitForIdle ( );
            ImGuiController.Dispose ( );
            CommandList.Dispose ( );
            GraphicsDevice.Dispose ( );
            BaseOverlay.Dispose ( );
            Sdl2Window.Close ( );
            HasExited = true;
        }
    }
}
