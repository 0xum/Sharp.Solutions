using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Sharp.ImGui.Wpf
{
    public static class ImCheckBoxExtension
    {
        public static async Task<bool> CheckBox ( this ImGuiWpf imGui, string text, bool isChecked )
        {
            var checkBox = await imGui.HandleControl<Controls.ImCheckBox>(new object[] { text, isChecked });
            return checkBox.GetState<bool?> ( "Checked" ) ?? false;
        }

        public static async Task<bool> CheckBox ( this ImGuiWpf imGui, string text, bool isChecked, string toolTip = null )
        {
            var checkBox = await imGui.HandleControl<Controls.ImCheckBox>(new object[] { text, isChecked });

            if ( toolTip != null )
            {
                checkBox.WindowsControl.MouseMove += OnMouseMove;
                m_toolTip = toolTip.ToString ( );
            }

            return checkBox.GetState<bool?> ( "Checked" ) ?? false;
        }

        public static async Task<bool> Toggle ( this ImGuiWpf imGui, string text, bool isChecked )
        {
            return await imGui.CheckBox ( text, isChecked );
        }

        public static async Task<bool> Toggle ( this ImGuiWpf imGui, string text, bool isChecked, string toolTip = null )
        {
            return await imGui.CheckBox ( text, isChecked, toolTip );
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
    internal class ImCheckBox : IImGuiControl
    {
        private readonly CheckBox m_checkBox;
        public FrameworkElement WindowsControl => m_checkBox;

        private bool? m_isChecked;
        private bool? m_lastKnownChecked;

        public ImCheckBox ( )
        {
            m_checkBox = new CheckBox
            {
                IsThreeState = false,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            m_checkBox.Checked += OnChecked;
            m_checkBox.Unchecked += OnUnchecked;
        }

        private void OnUnchecked ( object sender, RoutedEventArgs e )
        {
            m_isChecked = false;
        }

        private void OnChecked ( object sender, RoutedEventArgs e )
        {
            m_isChecked = true;
        }

        public void Update ( object [ ] data )
        {
            m_checkBox.Content = ( string ) data [ 0 ];

            var checkState = (bool?) data[1];
            if ( checkState == m_lastKnownChecked )
            {
                return;
            }

            m_lastKnownChecked = m_isChecked = checkState;
            m_checkBox.IsChecked = m_isChecked;
        }

        public void ApplyStyle ( IImGuiStyle style )
        {
            m_checkBox.Padding = style.Padding;
            m_checkBox.Margin = style.Margin;
        }

        public TResult GetState<TResult> ( string stateName )
        {
            if ( stateName == "Checked" )
            {
                m_lastKnownChecked = m_isChecked;
                return ImGuiWpfExtensions.Cast<TResult> ( m_isChecked );
            }

            return default ( TResult );
        }
    }
}
