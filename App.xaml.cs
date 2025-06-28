using PedidoApp;
using Steady_Management.WPF.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Steady_Management_App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new MainWindow().Show(); // Ventana principal real

        }

    }



}
