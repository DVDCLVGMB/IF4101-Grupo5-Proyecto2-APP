// ClientViewModel.cs (actualizado para manejar Guardar y Eliminar)
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Steady_Management_App.Models;
using Steady_Management_App.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Steady_Management_App.ViewModels
{
    public class ClientViewModel : ObservableObject
    {
        private readonly ClientApiService _apiService;

        public ObservableCollection<Client> Clients { get; set; } = new();

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
            _apiService = new ClientApiService();
            SaveClientCommand = new AsyncRelayCommand(SaveClientAsync);
            DeleteClientCommand = new AsyncRelayCommand(DeleteClientAsync);
            _ = LoadClientsAsync();
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
                CityId = 1,                      // ← Asegúrate que este ID exista en la tabla City
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
            if (SelectedClient == null) return;

            if (SelectedClient.ClientId > 0)
            {
                await _apiService.UpdateClientAsync(SelectedClient);
            }
            else
            {
                await _apiService.AddClientAsync(SelectedClient);
            }

            await LoadClientsAsync();
            SelectedClient = null;
        }

        private async Task DeleteClientAsync()
        {
            if (SelectedClient == null) return;

            await _apiService.DeleteClientAsync(SelectedClient.ClientId);
            await LoadClientsAsync();
            SelectedClient = null;
        }
    }
}
