using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ImGui.Wpf;


namespace Imortal.FluentFramework.Views.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Skins : Page
    {
        public Skins ( )
        {
            InitializeComponent ( );

            Task.Run ( async ( ) => await UpdateImDemo ( MainGp ) );
        }

        private async Task UpdateImDemo ( GroupBox owner )
        {
            var sliderValue = 0.5;
            var buffer = "Hello World";

            // Create an instance of an imGui panel.
            // If no parameter is passed, the main application window is used as the
            // hosting control, however you can pass any FrameworkElement which implements
            // IAddChild as a hosting control.

            using ( var imGui = await ImGuiWpf.BeginUi ( owner ) )
            {
                while ( true )
                {
                    // All ImGui.Wpf calls must be wrapped in a Begin and End frame.
                    imGui.BeginFrame ( );

                    // Add a TextBlock to the UI.
                    await imGui.Text ( "Hello World {0}", 123 );

                    // Add a button to the UI, which will return true if the button was clicked.
                    if ( await imGui.Button ( "Save", "Save using that button! :)" ) )
                    {
                        MessageBox.Show ( $"You clicked save!\n{buffer}" );
                    }

                    // Add a TextBox control, because we are async we can't use 'ref' so have to 
                    // pass in the variable and return the potentially updated contents.
                    buffer = await imGui.InputText ( "Input Text:", buffer );

                    // Add a Slider control with a value, minimum and maximum.
                    sliderValue = await imGui.Slider ( "Slider:", sliderValue, 0.0, 10.0, "Slider Value Here :)" );


                    // End the frame.
                    await imGui.EndFrame ( );

                    await Task.Delay ( 50 );
                }
            }
        }

    }
}
