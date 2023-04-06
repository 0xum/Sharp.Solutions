using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Sharp.ImGui.Wpf
{
    public static class ImSliderExtension
    {
        public static async Task<double> Slider ( this ImGuiWpf imGui, string title, double value, double minimum, double maximum )
        {
            var slider = await imGui.HandleControl<Controls.ImSlider>(new object[] { title, value, minimum, maximum });
            return slider.GetState<double> ( "Value" );
        }
        public static async Task<double> Slider ( this ImGuiWpf imGui, string title, double value, double minimum, double maximum, string toolTip = null )
        {
            var slider = await imGui.HandleControl<Controls.ImSlider>(new object[] { title, value, minimum, maximum });

            if ( toolTip != null )
            {
                slider.WindowsControl.MouseMove += OnMouseMove;
                m_toolTip = toolTip.ToString ( );
            }

            return slider.GetState<double> ( "Value" );
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
    internal class ImSlider : IImGuiControl
    {
        private readonly DockPanel m_dockPanel;
        private readonly TextBlock m_label;
        private readonly Slider m_slider;
        private readonly TextBox m_editBox;

        public FrameworkElement WindowsControl => m_dockPanel;

        private double? m_lastKnownValue;
        private double m_sliderValue;

        public ImSlider ( )
        {
            m_dockPanel = new DockPanel ( );

            m_label = new TextBlock ( );

            m_slider = new Slider ( );
            m_slider.ValueChanged += OnValueChanged;

            m_slider.AutoToolTipPlacement = AutoToolTipPlacement.TopLeft;
            m_slider.AutoToolTipPrecision = 2;

            m_editBox = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center
            };
            m_editBox.TextChanged += OnTextChanged;

            DockPanel.SetDock ( m_label, Dock.Left );
            DockPanel.SetDock ( m_editBox, Dock.Right );

            m_dockPanel.Children.Add ( m_label );
            m_dockPanel.Children.Add ( m_editBox );
            m_dockPanel.Children.Add ( m_slider );
        }

        private void OnTextChanged ( object sender, TextChangedEventArgs e )
        {
            if ( double.TryParse ( m_editBox.Text, out var newValue ) && !Double.IsNaN ( newValue ) && !Double.IsInfinity ( newValue ) )
            {

                if ( newValue > m_slider.Maximum )
                {
                    m_slider.Value = m_slider.Maximum;
                    m_sliderValue = m_slider.Maximum;
                    m_editBox.Text = m_sliderValue.ToString ( "F" );
                }
                else if ( newValue < m_slider.Minimum )
                {
                    m_slider.Value = m_slider.Minimum;
                    m_sliderValue = m_slider.Minimum;
                    m_editBox.Text = m_sliderValue.ToString ( "F" );
                }
                else
                {
                    m_slider.Value = newValue;
                    m_sliderValue = newValue;
                }
            }
            else
            {
                m_slider.Value = m_sliderValue;
                m_editBox.Text = m_sliderValue.ToString ( "F" );
            }
        }

        private void OnValueChanged ( object sender, RoutedPropertyChangedEventArgs<double> e )
        {
            m_sliderValue = m_slider.Value;
            m_editBox.Text = m_sliderValue.ToString ( "F" );
        }


        public void Update ( object [ ] data )
        {
            m_label.Text = ( string ) data [ 0 ];

            var value = (double)data[1];
            var minimum = (double)data[2];
            var maximum = (double)data[3];

            m_slider.Minimum = minimum;
            m_slider.Maximum = maximum;

            if ( value == m_lastKnownValue )
            {
                return;
            }

            m_lastKnownValue = m_sliderValue = value;
            m_slider.Value = m_sliderValue;
            m_editBox.Text = m_sliderValue.ToString ( "F" );
        }

        public void ApplyStyle ( IImGuiStyle style )
        {
            m_label.Padding = style.Padding;
            m_label.Margin = style.Margin;
            m_slider.Padding = style.Padding;
            m_slider.Margin = style.Margin;
            m_editBox.Padding = style.Padding;
            m_editBox.Margin = style.Margin;
        }

        public TResult GetState<TResult> ( string stateName )
        {
            if ( stateName == "Value" )
            {
                m_lastKnownValue = m_sliderValue;
                return ImGuiWpfExtensions.Cast<TResult> ( m_sliderValue );
            }

            return default ( TResult );
        }
    }
}
