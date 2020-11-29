using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CrypterDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();

            if (e.Args.Length > 0)
            {
                try
                {
                    mainWindow.OpenFile(e.Args[0]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace);
                }
            }

            mainWindow.Show();
        }

        public static void Run(Action action)
        {
            Current.Dispatcher.InvokeAsync(() =>
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    HandleException(e);
                }
            });
        }

        public static void HandleException(Exception e)
        {
            MessageBox.Show($"{e.GetType()} {e.StackTrace}");
        }
    }
}