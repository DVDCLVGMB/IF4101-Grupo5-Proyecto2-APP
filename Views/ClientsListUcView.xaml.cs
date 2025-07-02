using PedidoApp;
using Steady_Management.WPF.Views;
using Steady_Management_App.Models;
using Steady_Management_App.ViewModels;
using Steady_Management_App.Views;
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
        }
        public ClientsListUcView(ClientViewModel vm) : this()
        {
            DataContext = vm;
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
