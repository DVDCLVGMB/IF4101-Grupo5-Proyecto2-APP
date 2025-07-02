using System.Windows;
using PedidoApp;
using Steady_Management_App.Views;    

namespace Steady_Management_App
{


    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();  // importa mucho para que cargue el App.xaml
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            // 1) Evitamos shutdown automático al cerrar ventanas
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            bool loggedIn = false;
            while (!loggedIn)
            {
                var login = new LoginWindow();
                bool? ok = login.ShowDialog();

                // sólo salimos cuando nos devuelva true
                loggedIn = ok == true;
            }

            // 2) Login OK → volvemos al modo normal
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;

            // 3) Por fin instanciamos y mostramos tu MainWindow
            var main = new MainWindow();
            Current.MainWindow = main;
            main.Show();
        }
    }


}