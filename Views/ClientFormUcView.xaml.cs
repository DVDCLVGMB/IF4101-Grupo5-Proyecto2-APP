using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Steady_Management_App.ViewModels;  // para ClientViewModel
using Steady_Management_App.Views;       // para ClientsListUcView

namespace Steady_Management_App.Views
{
    public partial class ClientFormUcView : UserControl
    {
        // Exponemos el VM para poder reutilizarlo al cancelar
        public ClientViewModel Vm { get; }

        // Sólo un constructor que reciba el VM
        public ClientFormUcView(ClientViewModel viewModel)
        {
            InitializeComponent();
            DataContext = Vm = viewModel;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            var listView = new ClientsListUcView(Vm);
            // Reemplazamos el contenido del MDI
            Application.Current.Windows
                .OfType<PedidoApp.MainWindow>()
                .First()
                .ContenidoArea
                .Content = listView;
        }
    }
}
