using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace openMultiCam {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class OpenMultiCam : Application {
        private MainWindow mainWindow;

        private void Application_Startup(object sender, StartupEventArgs e) {
            if(!Directory.Exists("omcWorkspace")) {
                Directory.CreateDirectory("omcWorkspace");
            }

            mainWindow = new MainWindow(this);
            mainWindow.Show();
        }

    }
}
