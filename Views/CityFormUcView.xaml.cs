// CityFormUcView.xaml.cs
using CommunityToolkit.Mvvm.Input;
using PedidoApp;
using Steady_Management_App.ViewModels;
using Steady_Management_App.Views;   
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Steady_Management_App.Views
{
    public partial class CityFormUcView : UserControl
    {
        public CityViewModel Vm { get; }

        public CityFormUcView(CityViewModel vm)
        {
            InitializeComponent();
            DataContext = Vm = vm;
        }

        private async void OnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Ejecutamos el comando de guardado
                await ((AsyncRelayCommand)Vm.SaveCityCommand).ExecuteAsync(null);

                // Navegamos de vuelta a la lista, pasando el mismo VM
                var lista = new CitiesListUcView(Vm);
                ((MainWindow)Application.Current.MainWindow)
                    .ContenidoArea.Content = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al guardar la ciudad:\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        private void OnCancel_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow)
                .ContenidoArea.Content = new CitiesListUcView(Vm);
        }
    }
}
