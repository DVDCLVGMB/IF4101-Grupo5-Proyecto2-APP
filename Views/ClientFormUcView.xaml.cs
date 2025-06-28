using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Steady_Management_App.Views
{
    public partial class ClientFormUcView : UserControl
    {
        public ClientFormUcView()
        {
            InitializeComponent();
        }

        public ClientFormUcView(ViewModels.ClientViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.Windows
                .OfType<PedidoApp.MainWindow>()
                .FirstOrDefault();

            if (mainWindow != null)
            {
                mainWindow.ContenidoArea.Content = new ClientsListUcView();
            }
        }
    }
}
