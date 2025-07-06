using System.Windows;
using System.Windows.Controls;
using PedidoApp;
using Steady_Management_App.Models;
using Steady_Management_App.ViewModels;
using Steady_Management_App.ViewModels.Utility;

namespace Steady_Management_App.Views
{
    public partial class OrderClientSelectUcView : UserControl
    {
        public Client? SelectedClient { get; private set; }

        private readonly ClientViewModel _viewModel;

        public OrderClientSelectUcView()
        {
            InitializeComponent();
            _viewModel = new ClientViewModel();
            DataContext = _viewModel;
        }

        // Seleccionar cliente (botón arriba)
        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedClient != null)
            {
                SelectedClient = _viewModel.SelectedClient;

                // Oculta la tabla, muestra resumen
                ClientDataGrid.Visibility = Visibility.Collapsed;
                SelectedClientPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Seleccione un cliente primero.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Cambiar cliente (volver a tabla)
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            SelectedClient = null;
            _viewModel.SelectedClient = null;

            // Muestra la tabla, oculta resumen
            ClientDataGrid.Visibility = Visibility.Visible;
            SelectedClientPanel.Visibility = Visibility.Collapsed;
        }

        // Continuar (ir a productos)
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedClient != null)
            {
                // Guarda el cliente en la "sesión" del pedido
                OrderSession.SelectedClient = SelectedClient;

                var productosUc = new OrderProductSelectUcView();

                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.ContenidoArea.Content = productosUc;
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un cliente primero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
