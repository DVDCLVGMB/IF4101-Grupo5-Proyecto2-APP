using System.Configuration;
using System.Data;
using System.Windows;
using Steady_Management.WPF.Views;

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
            new EmployeesView().Show(); // Ventana principal
        }

    }



}
