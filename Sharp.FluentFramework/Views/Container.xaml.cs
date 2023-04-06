using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using System.Windows;


namespace Imortal.FluentFramework.Views
{
    /// <summary>
    /// Interaction logic for Container.xaml
    /// </summary>
    public partial class Container : Window
    {
        public Container ( )
        {
            InitializeComponent ( );

            Loaded += ( sender, args ) =>
            {
                Watcher.Watch (
                  this,                           // Window class
                  BackgroundType.Acrylic,         // Background type
                  true                            // Whether to change accents automatically
                );
            };
        }
    }
}