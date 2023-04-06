using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Sharp.ImGui.Wpf
{
    public static class ImButtonExtension
    {
        public static async Task<bool> Button ( this ImGuiWpf imGui, string text )
        {
            var button = await imGui.HandleControl<Controls.ImButton>(new object[] { text });
            return button.GetState<bool> ( "Clicked" );
        }

        public static async Task<bool> Button ( this ImGuiWpf imGui, string text, object toolTip )
        {
            var button = await imGui.HandleControl<Controls.ImButton>(new object[] { text });
            if (toolTip != null )
            {
                button.WindowsControl.MouseMove += OnMouseMove;

                m_toolTip = toolTip.ToString ( );
            }

            return button.GetState<bool> ( "Clicked" );
        }

        static string m_toolTip;

        static void OnMouseMove ( object sender, System.Windows.Input.MouseEventArgs e )
        {
            if ( ( sender as FrameworkElement ).ToolTip == null )
                ( sender as FrameworkElement ).ToolTip = new ToolTip ( ) { Placement = PlacementMode.Relative };
            double x = e.GetPosition((sender as FrameworkElement)).X;
            double y = e.GetPosition((sender as FrameworkElement)).Y;
            var tip = ((sender as FrameworkElement).ToolTip as ToolTip);
            tip.Content = m_toolTip;
            tip.HorizontalOffset = x + 10;
            tip.VerticalOffset = y + 10;
        }
    }
}

namespace Sharp.ImGui.Wpf.Controls
{
    internal class ImButton : IImGuiControl
    {
        private readonly Button m_button;
        public FrameworkElement WindowsControl => m_button;

        private bool m_isClicked;

        public ImButton ( )
        {
            m_button = new Button ( );

            m_button.HorizontalAlignment = HorizontalAlignment.Stretch;
            m_button.VerticalAlignment = VerticalAlignment.Stretch;

            m_button.Click += OnButtonClick;
        }

        private void OnButtonClick ( object sender, RoutedEventArgs e )
        {
            m_isClicked = true;
        }

        public void Update ( object [ ] data )
        {
            m_button.Content = ( string ) data [ 0 ];
        }

        public void ApplyStyle ( IImGuiStyle style )
        {
            m_button.Padding = style.Padding;
            m_button.Margin = style.Margin;
        }

        public TResult GetState<TResult> ( string stateName )
        {
            if ( stateName == "Clicked" )
            {
                if ( m_isClicked )
                {
                    m_isClicked = false;
                    return ImGuiWpfExtensions.Cast<TResult> ( true );
                }

                return ImGuiWpfExtensions.Cast<TResult> ( false );
            }

            return default ( TResult );
        }
    }
}
