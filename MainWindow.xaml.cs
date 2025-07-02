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
            Loaded += MainWindow_Loaded; // Esperar a que cargue para configurar
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ConfigurarSegunRol();
        }

        private void ConfigurarSegunRol()
        {
            string rol = App.Current.Properties["UserRole"]?.ToString() ?? "Invitado";
            string usuario = App.Current.Properties["UserName"]?.ToString() ?? "Desconocido";

            //UsuarioLabel.Text = $"Usuario: {usuario} - Rol: {rol}";
            int.TryParse(rol, out int roleId);

            // Rol 21: Empleado
            if (roleId == 21)
            {
                // Ocultar menús
                MantenimientoMenu.Visibility = Visibility.Collapsed;
                ReportesMenu.Visibility = Visibility.Collapsed;
                ParametrosMenu.Visibility = Visibility.Collapsed;

                // Ocultar botones de la barra
                btnClientes.Visibility = Visibility.Collapsed;
                btnInventario.Visibility = Visibility.Collapsed;
                btnCiudades.Visibility = Visibility.Collapsed;
                btnReportes.Visibility = Visibility.Collapsed;
            }
        }


        // ===================== CRUDs =====================
        private void AbrirClientes(object sender, RoutedEventArgs e)
            => ContenidoArea.Content = new ClientsListUcView();

        private void AbrirProductos(object sender, RoutedEventArgs e)
            => ContenidoArea.Content = new ProductListUCView();

        private void AbrirEmpleados(object sender, RoutedEventArgs e)
            => ContenidoArea.Content = new EmployeesListUcView();

        private void AbrirDepartamentos(object sender, RoutedEventArgs e)
        {
            ContenidoArea.Content = new DepartmentFormUcView
            {
                DataContext = new DepartmentFormViewModel()
            };
        }

  

        private void AbrirCiudades(object sender, RoutedEventArgs e)
    => ContenidoArea.Content = new CitiesListUcView();

        private void AbrirCrearPedido(object sender, RoutedEventArgs e)
        {
            ContenidoArea.Content = new OrderCreateWindow(); // Asegurate que la clase UserControl exista
        }

        private void AbrirListaPedidos(object sender, RoutedEventArgs e)
        {
            ContenidoArea.Content = new Order(); // Cambialo si usás otro nombre para la vista de lista
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
            => ContenidoArea.Content = new ParameterUcView();

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
