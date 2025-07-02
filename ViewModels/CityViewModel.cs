using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Steady_Management.WPF.Views;
using Steady_Management_App.Models;
using Steady_Management_App.Services;
using Steady_Management_App.Views;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Steady_Management_App.ViewModels
{
    public class CityViewModel : ObservableObject
    {
        private readonly CityApiService _service;
        public ObservableCollection<City> Cities { get; } = new();
        private City _selectedCity;
        public City SelectedCity
        {
            get => _selectedCity;
            set => SetProperty(ref _selectedCity, value);
        }

        public IAsyncRelayCommand LoadCitiesCommand { get; }
        public IAsyncRelayCommand SaveCityCommand { get; }
        public IAsyncRelayCommand DeleteCityCommand { get; }

        public CityViewModel()
        {
            _service = new CityApiService();
            LoadCitiesCommand = new AsyncRelayCommand(LoadCitiesAsync);
            SaveCityCommand = new AsyncRelayCommand(SaveCityAsync);
            DeleteCityCommand = new AsyncRelayCommand(DeleteCityAsync);

            // Carga inicial
            _ = LoadCitiesAsync();
        }

        private async Task LoadCitiesAsync()
        {
            var list = await _service.GetCitiesAsync();
            Cities.Clear();
            foreach (var c in list) Cities.Add(c);
        }

        private async Task SaveCityAsync()
        {
            if (SelectedCity == null) return;

            // Validación local
            if (string.IsNullOrWhiteSpace(SelectedCity.CityName))
            {
                Application.Current.Dispatcher.Invoke(() =>
                    new ValidationDialog().ShowDialog());
                return;
            }

            if (SelectedCity.CityId > 0)
            {
                //  UPDATE 
                try
                {
                    await _service.UpdateCityAsync(SelectedCity);
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                }
                catch (HttpRequestException ex)
                {
                    // Cualquier otro fallo de HTTP
                    MessageBox.Show(
                        $"Error al actualizar la ciudad:\n{ex.StatusCode ?? 0} {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                //  CREATE 
                try
                {
                    SelectedCity = await _service.AddCityAsync(SelectedCity);
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show(
                        $"Error al crear la ciudad:\n{ex.StatusCode ?? 0} {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }

            // Refrescamos la lista
            await LoadCitiesAsync();
            SelectedCity = null;
        }



        private async Task DeleteCityAsync()
        {
            if (SelectedCity == null) return;

            HttpResponseMessage resp;
            try
            {
                resp = await _service.DeleteCityAsync(SelectedCity.CityId);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error de red:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (resp.IsSuccessStatusCode || resp.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Si fue 204 o incluso 404 , refrescamos la lista
                await LoadCitiesAsync();
                SelectedCity = null;
            }
            else
            {
                MessageBox.Show($"Error al eliminar:\n{resp.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
