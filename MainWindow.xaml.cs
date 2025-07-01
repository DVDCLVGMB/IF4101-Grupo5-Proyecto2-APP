using Steady_Management_App;
using Steady_Management_App.Models;
using Steady_Management_App.ViewModels;
using Steady_Management_App.Views;
using System.Windows;

namespace PedidoApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ConfigurarSegunRol();
        }

        private void ConfigurarSegunRol()
        {
            string rol = App.Current.Properties["UserRole"]?.ToString() ?? "Invitado";
            string usuario = App.Current.Properties["UserName"]?.ToString() ?? "Desconocido";

            //UsuarioLabel.Text = $"Usuario: {usuario} - Rol: {rol}";

            // Personaliza visibilidad de menús según rol
            if (rol == "Vendedor")
            {
                MantenimientoMenu.Visibility = Visibility.Collapsed;
                ReportesMenu.Visibility = Visibility.Collapsed;
                ParametrosMenu.Visibility = Visibility.Collapsed;
            }
        }

        // ==== CRUDs ====
        private void AbrirClientes(object sender, RoutedEventArgs e)
            => ContenidoArea.Content = new ClientsListUcView();

        // private void AbrirProductos(object sender, RoutedEventArgs e)
        //   => ContenidoArea.Content = new ProductosView();




        private void AbrirEmpleados(object sender, RoutedEventArgs e)
            => ContenidoArea.Content = new EmployeesListUcView();



        private void AbrirDepartamentos(object sender, RoutedEventArgs e)
        {
            ContenidoArea.Content = new DepartmentFormUcView
            {
                DataContext = new DepartmentFormViewModel()
            };
        }

        private void AbrirProductos(object sender, RoutedEventArgs e)
            => ContenidoArea.Content = new ProductListUCView();
        

        private void AbrirClientesForm()
        {
            ContenidoArea.Content = new Steady_Management_App.Views.ClientFormUcView();
        }



         

        private void AbrirPedido(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Función Pedido aún no implementada.");
        }
        private void AbrirReporteVentas(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Función Reporte Ventas aún no implementada.");
        }
        private void AbrirReporteClientes(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Función Reporte Clientes aún no implementada.");
        }
        private void AbrirReporteProductos(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Función Reporte Productos aún no implementada.");
        }
        private void AbrirParametros(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Función Parámetros aún no implementada.");
        }

        /*
        // ==== PEDIDOS ====
        private void AbrirPedido(object sender, RoutedEventArgs e)
            => ContenidoArea.Content = new PedidoView();

        // ==== REPORTES ====
        private void AbrirReporteVentas(object sender, RoutedEventArgs e)
            => ContenidoArea.Content = new ReporteVentasView();

        private void AbrirReporteClientes(object sender, RoutedEventArgs e)
            => ContenidoArea.Content = new ReporteClientesView();

        private void AbrirReporteProductos(object sender, RoutedEventArgs e)
            => ContenidoArea.Content = new ReporteProductosView();

        //  PARÁMETROS 
        private void AbrirParametros(object sender, RoutedEventArgs e)
            => ContenidoArea.Content = new ParametrosView();

        //  SESIÓN 
        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        */

        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            bool? ok = login.ShowDialog();
            if (ok == true)
            {
                ConfigurarSegunRol();
                ContenidoArea.Content = null;
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
    
        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
