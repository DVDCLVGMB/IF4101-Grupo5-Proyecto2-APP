using PedidoApp;
using Steady_Management_App.Models;
using Steady_Management_App.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Steady_Management_App.Views
{
    public partial class CitiesListUcView : UserControl
    {
        public CityViewModel Vm => (CityViewModel)DataContext;
        public CitiesListUcView()
        {
            InitializeComponent();
        }

        public CitiesListUcView(CityViewModel vm) : this()
        {
            DataContext = vm;
        }
        private void OnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Vm.SelectedCity = new City();
            NavigateToForm();
        }

        private void OnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (Vm.SelectedCity != null)
                NavigateToForm();
        }

        private void NavigateToForm()
        {
            var form = new CityFormUcView(Vm);
            ((MainWindow)Application.Current.MainWindow)
                .ContenidoArea.Content = form;
        }
    }
}
