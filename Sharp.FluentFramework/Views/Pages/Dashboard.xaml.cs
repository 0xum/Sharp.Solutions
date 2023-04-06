using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    public partial class Dashboard : Page
    {
        public Dashboard ( )
        {
            InitializeComponent ( );

            ToolTipService.InitialShowDelayProperty.OverrideMetadata ( typeof ( FrameworkElement ), new FrameworkPropertyMetadata ( 1 ) );
            ToolTipService.BetweenShowDelayProperty.OverrideMetadata ( typeof ( FrameworkElement ), new FrameworkPropertyMetadata ( 1 ) );

            Task.Run ( async ( ) => await UpdateImDemo ( MainGp ) );
        }

        private async Task UpdateImDemo ( GroupBox owner )
        {
            var selectedFruit = "Banana";

            using ( var imGui = await ImGuiWpf.BeginUi ( owner ) )
            {
                while ( true )
                {
                    imGui.BeginFrame ( );

                    var content = new string [] { "Cranberry", "Banana", "Banana", "Banana", "Banana", "Banana" };

                    selectedFruit = await imGui.ComboBox ( "Fruits", selectedFruit, "Pick a fruit.", content );

                    await imGui.EndFrame ( );

                    await Task.Delay ( 50 );
                }
            }
        }
    }
}
