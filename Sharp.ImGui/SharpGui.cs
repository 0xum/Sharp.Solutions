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

#pragma warning disable CS8618 

namespace Sharp.ImGui
{
    public class SharpGui
    {

        public static SharpGui Instance;

        public virtual void OnVisible ( ) { }

        public virtual void OnHidden ( ) { }

        public SharpGui ( Process process, float framesPerSecond = 60f, bool isVisible = true )
        {

            OverlayProcess ( process, framesPerSecond );

            IsVisible = isVisible;
            Instance = this;

        }

        public bool IsVisible;

        public Sdl2Window Sdl2Window;
        public GraphicsDevice GraphicsDevice;
        public CommandList CommandList;
        public ImGuiController ImGuiController;
        public Process TargetProcess;
        public System.Drawing.Rectangle WindowSize;
        public VirtualKeys ToggleVisibilityKey = VirtualKeys.Insert, EscapeKey = VirtualKeys.End;
        public Vector4 BackgroundColor = new Vector4(0.0f, 0.00f, 0.00f, 0.55f);
        public Vector4 ClearColor = new Vector4(0f, 0f, 0f, 0f);

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


        void HandleVisibility ( )
        {

            if ( WindowManager.CurrentApplicationIsActivated ( ) || WindowManager.ApplicationIsActivated ( TargetProcess.Id ) )
            {

                if ( Input.IsKeyDown ( ToggleVisibilityKey ) )
                {

                    IsVisible = !IsVisible;

                    //if ( IsVisible )
                    //{

                    //    Windows.SetCurrentWindowAsForeground ( );

                    //    Input.SetCursorPosition ( WindowSize.X + WindowSize.Width / 2, WindowSize.Y + WindowSize.Height / 2 );

                    //    Input.MouseEvent ( MouseEventFlags.LeftDown );
                    //    Input.MouseEvent ( MouseEventFlags.LeftUp );

                    //}
                    //else
                    //{

                    //    Windows.SetWindowAsForeground ( TargetProcess.MainWindowHandle );
                    //}
                }
            }

            WindowManager.ManageClickThrough ( Sdl2Window.Handle, !IsVisible, true );
        }

        void FitToProcess ( )
        {
            var size = WindowManager.GetWindowSize(TargetProcess.MainWindowHandle);

            if ( Sdl2Window != null )
            {
                if ( WindowSize != size )
                {

                    Sdl2Window.X = size.X;
                    Sdl2Window.Y = size.Y;
                    Sdl2Window.Width = size.Width;
                    Sdl2Window.Height = size.Height;

                    WindowSize = size;
                }
            }
        }

        public void OverlayProcess ( Process process, float framesPerSecond )
        {

            TargetProcess = process;

            FitToProcess ( );

            var graphicsDeviceOptions = new GraphicsDeviceOptions(true, null, true);

            Sdl2Window = new Sdl2Window ( string.Empty, WindowSize.X, WindowSize.Y, WindowSize.Width, WindowSize.Height, SDL_WindowFlags.SkipTaskbar | SDL_WindowFlags.Borderless | SDL_WindowFlags.AlwaysOnTop, true );

            GraphicsDevice = VeldridStartup.CreateGraphicsDevice ( Sdl2Window, graphicsDeviceOptions );

            Sdl2Window.Resized += HandleReResize;

            CommandList = GraphicsDevice.ResourceFactory.CreateCommandList ( );

            ImGuiController = new ImGuiController ( GraphicsDevice, GraphicsDevice.MainSwapchain.Framebuffer.OutputDescription, Sdl2Window.Width, Sdl2Window.Height );

            while ( Sdl2Window.Exists )
            {
                WindowManager.EnableTransparency ( Sdl2Window.Handle, WindowSize );

                if ( !Sdl2Window.Exists )
                    break;

                FitToProcess ( );
                HandleVisibility ( );

                ImGuiController.Update ( 1f / framesPerSecond, Sdl2Window.PumpEvents ( ) );

                CommandList.Begin ( );

                try
                { CommandList.SetFramebuffer ( GraphicsDevice.MainSwapchain.Framebuffer ); }
                catch { }


                if ( WindowManager.ApplicationIsActivated ( TargetProcess.Id ) || WindowManager.CurrentApplicationIsActivated ( ) )
                {
                    if ( IsVisible )
                        OnVisible ( );
                    else
                        OnHidden ( );
                }

                try
                {

                    if ( WindowManager.CurrentApplicationIsActivated ( ) || WindowManager.ApplicationIsActivated ( TargetProcess.Id ) )
                    {

                        if ( IsVisible )
                            CommandList.ClearColorTarget ( 0, new RgbaFloat ( BackgroundColor.X, BackgroundColor.Y, BackgroundColor.Z, BackgroundColor.W ) );
                        else
                            CommandList.ClearColorTarget ( 0, new RgbaFloat ( ClearColor.X, ClearColor.Y, ClearColor.Z, ClearColor.W ) );

                    }
                    else
                        CommandList.ClearColorTarget ( 0, new RgbaFloat ( ClearColor.X, ClearColor.Y, ClearColor.Z, ClearColor.W ) );

                }
                catch { }

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
        }
    }
}
