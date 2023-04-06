using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Imortal.FluentFramework
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App ( )
        {

            Wpf.Ui.Appearance.Theme.Apply (
              Wpf.Ui.Appearance.ThemeType.Dark,     // Theme type
              Wpf.Ui.Appearance.BackgroundType.Acrylic, // Background type
              true                                  // Whether to change accents automatically
            );

        }

    }
}
