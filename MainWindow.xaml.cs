using Steady_Management_App;
using Steady_Management_App.Models;
using Steady_Management_App.ViewModels;
using Steady_Management_App.Views;
using System.Windows;

namespace PedidoApp
{
    public partial class MainWindow : Window
    {
        private int roleId;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ConfigurarSegunRol();
        }

        private void ConfigurarSegunRol()
        {
            string rol = App.Current.Properties["UserRole"]?.ToString() ?? "Invitado";
            string usuario = App.Current.Properties["UserName"]?.ToString() ?? "Desconocido";

            int.TryParse(rol, out roleId);

            if (roleId == 21) // Empleado
            {
                // Ocultar completamente secciones para empleados
                MantenimientoMenu.Visibility = Visibility.Collapsed;
                ReportesMenu.Visibility = Visibility.Collapsed;
                ParametrosMenu.Visibility = Visibility.Collapsed;

                btnClientes.Visibility = Visibility.Collapsed;
                btnInventario.Visibility = Visibility.Collapsed;
                btnCiudades.Visibility = Visibility.Collapsed;
                btnReportes.Visibility = Visibility.Collapsed;
            }
        }

        // ===================== CRUDs =====================
        private void AbrirClientes(object sender, RoutedEventArgs e)
        {
            if (roleId == 21) return;
            ContenidoArea.Content = new ClientsListUcView();
        }

        private void AbrirProductos(object sender, RoutedEventArgs e)
        {
            if (roleId == 21) return;
            ContenidoArea.Content = new ProductListUCView();
        }

        private void AbrirEmpleados(object sender, RoutedEventArgs e)
        {
            if (roleId == 21) return;
            ContenidoArea.Content = new EmployeesListUcView();
        }

        private void AbrirDepartamentos(object sender, RoutedEventArgs e)
        {
            ContenidoArea.Content = new DepartmentFormUcView
            {
                DataContext = new DepartmentFormViewModel()
            };
        }

        private void AbrirCiudades(object sender, RoutedEventArgs e)
        {
            if (roleId == 21) return;
            ContenidoArea.Content = new CitiesListUcView();
        }

        private void AbrirCrearPedido(object sender, RoutedEventArgs e)
           => ContenidoArea.Content = new OrderClientSelectUcView();


        private void AbrirListaPedidos(object sender, RoutedEventArgs e)
        {
            if (roleId == 21) return;                        
            ContenidoArea.Content = new OrderListUcView(); 
        }

        private void AbrirInventario(object sender, RoutedEventArgs e)
        {
            ContenidoArea.Content = new InventoryListUcView();
        }

        private void AbrirPedido(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Función Pedido aún no implementada.");
        }

        private void AbrirReporteVentas(object sender, RoutedEventArgs e)
        {
            if (roleId == 21) return;
            MessageBox.Show("Función Reporte Ventas aún no implementada.");
        }

        private void AbrirReporteClientes(object sender, RoutedEventArgs e)
        {
            if (roleId == 21) return;
            MessageBox.Show("Función Reporte Clientes aún no implementada.");
        }

        private void AbrirReporteProductos(object sender, RoutedEventArgs e)
        {
            if (roleId == 21) return;
            MessageBox.Show("Función Reporte Productos aún no implementada.");
        }

        private void AbrirParametros(object sender, RoutedEventArgs e)
        {
            if (roleId == 21) return;
            ContenidoArea.Content = new ParameterUcView();
        }

        // ===================== SESIÓN =====================
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
