using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Steady_Management.WPF.Views;
using Steady_Management_App.Models;
using Steady_Management_App.Services;
using Steady_Management_App.Views;    
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;                  

namespace Steady_Management_App.ViewModels
{
    public class ClientViewModel : ObservableObject
    {
        private readonly ClientApiService _apiService;
        private readonly CityApiService _citySvc;

        public ObservableCollection<Client> Clients { get; } = new();
        public ObservableCollection<City> Cities { get; } = new();
        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set => SetProperty(ref _selectedClient, value);
        }

        public IAsyncRelayCommand SaveClientCommand { get; }
        public IAsyncRelayCommand DeleteClientCommand { get; }

        public ClientViewModel()
        {
            _citySvc = new CityApiService();
            _apiService = new ClientApiService();
            SaveClientCommand = new AsyncRelayCommand(SaveClientAsync);
            DeleteClientCommand = new AsyncRelayCommand(DeleteClientAsync);
            _ = LoadClientsAsync();
            _ = LoadCitiesAsync();
           
        }

        private async Task LoadCitiesAsync()
        {
            var list = await _citySvc.GetCitiesAsync();
            Cities.Clear();
            foreach (var c in list)
                Cities.Add(c);
        }

        public async Task LoadClientsAsync()
        {
            var list = await _apiService.GetClientsAsync();
            Clients.Clear();
            foreach (var c in list)
                Clients.Add(c);
        }

        public void PrepareNewClient()
        {
            SelectedClient = new Client
            {
                CityId = 1,
                CompanyName = "",
                ContactName = "",
                ContactSurname = "",
                ContactRank = "",
                ClientAddress = "",
                ClientPhoneNumber = "",
                ClientFaxNumber = "",
                ClientPostalCode = ""
            };
        }

        public void PrepareEditClient(Client client)
        {
            SelectedClient = new Client
            {
                ClientId = client.ClientId,
                CityId = client.CityId,
                CompanyName = client.CompanyName,
                ContactName = client.ContactName,
                ContactSurname = client.ContactSurname,
                ContactRank = client.ContactRank,
                ClientAddress = client.ClientAddress,
                ClientPhoneNumber = client.ClientPhoneNumber,
                ClientFaxNumber = client.ClientFaxNumber,
                ClientPostalCode = client.ClientPostalCode
            };
        }

        private async Task SaveClientAsync()
        {
            if (SelectedClient == null)
                return;

            // —— Validaciones de campos obligatorios ——
            if (string.IsNullOrWhiteSpace(SelectedClient.CompanyName) ||
                string.IsNullOrWhiteSpace(SelectedClient.ContactName) ||
                string.IsNullOrWhiteSpace(SelectedClient.ClientPhoneNumber) ||
                SelectedClient.CityId <= 0)
            {
                // Lanzar tu ventana de validación
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var dlg = new ValidationDialog();
                    dlg.ShowDialog();
                });
                return;
            }

            // —— Lógica normal de guardado ——
            if (SelectedClient.ClientId > 0)
                await _apiService.UpdateClientAsync(SelectedClient);
            else
                await _apiService.AddClientAsync(SelectedClient);

            await LoadClientsAsync();
            SelectedClient = null;
        }

        private async Task DeleteClientAsync()
        {
            if (SelectedClient == null)
                return;

            await _apiService.DeleteClientAsync(SelectedClient.ClientId);
            await LoadClientsAsync();
            SelectedClient = null;
        }

        public async Task DeleteClientAsync(int clientId)
        {
            await _apiService.DeleteClientAsync(clientId);
            await LoadClientsAsync();
        }

    }
}
