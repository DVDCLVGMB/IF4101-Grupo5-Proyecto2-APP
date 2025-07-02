using PedidoApp;
using Steady_Management.WPF.Views;
using Steady_Management_App.Models;
using Steady_Management_App.Models.Steady_Management_App.Models;
using Steady_Management_App.ViewModels;
using Steady_Management_App.Views;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Steady_Management_App
{
    public partial class ClientsListUcView : UserControl
    {
        private ClientViewModel ViewModel => DataContext as ClientViewModel;

        public ClientsListUcView()
        {
            InitializeComponent();
            DataContext = new ClientViewModel();
            Loaded += ClientsListUcView_Loaded;
        }

        public ClientsListUcView(ClientViewModel vm) : this()
        {
            DataContext = vm;
        }

        private void ClientsListUcView_Loaded(object sender, RoutedEventArgs e)
        {
            AplicarRestriccionesPorRol();
        }
        private void AplicarRestriccionesPorRol()
        {
            int roleId = UserSession.RoleId;

            if (roleId == 21) // Empleado
            {
                AgregarButton.Visibility = Visibility.Collapsed;

                // Ocultar columna "Acciones" manualmente
                // Removiendo la columna al cargar
                var gridView = ClientsListView.View as GridView;
                if (gridView != null && gridView.Columns.Count > 0)
                {
                    // Asumiendo que la columna "Acciones" es la última
                    gridView.Columns.RemoveAt(gridView.Columns.Count - 1);
                }
            }
        }

     
 


        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PrepareNewClient();
            var form = new ClientFormUcView(ViewModel);

            var mainWindow = Application.Current.Windows
                .OfType<PedidoApp.MainWindow>()
                .FirstOrDefault();

            if (mainWindow != null)
            {
                mainWindow.ContenidoArea.Content = form;
            }
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedClient == null)
            {
                MessageBox.Show("Por favor, seleccione un cliente antes de editar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            ViewModel.PrepareEditClient(ViewModel.SelectedClient);
            var form = new ClientFormUcView(ViewModel);

            var mainWindow = Application.Current.Windows
                .OfType<PedidoApp.MainWindow>()
                .FirstOrDefault();

            if (mainWindow != null)
            {
                mainWindow.ContenidoArea.Content = form;
            }
        }

        private async void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            var client = (Client)((FrameworkElement)sender).DataContext;

            var confirmDialog = new DeleteConfirmationDialog();
            confirmDialog.Owner = Window.GetWindow(this);
            confirmDialog.ShowDialog();

            if (confirmDialog.DeleteConfirmed)
            {
                await ViewModel.DeleteClientAsync(client.ClientId);
                await ViewModel.LoadClientsAsync();
            }
        }
    }
}
