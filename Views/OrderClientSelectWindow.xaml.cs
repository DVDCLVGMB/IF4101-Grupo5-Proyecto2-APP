using Steady_Management_App.Models;
using Steady_Management_App.ViewModels;
using System.Windows;

namespace Steady_Management_App.Views
{
    public partial class OrderClientSelectWindow : Window
    {
        public Client? SelectedClient { get; private set; }

        private readonly ClientViewModel _viewModel;

        public OrderClientSelectWindow()
        {
            InitializeComponent();
            _viewModel = new ClientViewModel();
            DataContext = _viewModel;
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedClient != null)
            {
                SelectedClient = _viewModel.SelectedClient;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Seleccione un cliente.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedClient != null)
            {
                SelectedClient = _viewModel.SelectedClient;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Seleccione un cliente.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}