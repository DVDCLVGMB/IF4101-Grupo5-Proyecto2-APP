using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public class ClientApiService
    {
        private static readonly HttpClient client = HttpClientProvider.Client;
        //private readonly string baseUrl = AppConfig.GetApiBaseUrl();

        public ClientApiService()
        {/*
            if (client.BaseAddress == null)
            {
                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new InvalidOperationException("La URL base de la API no está configurada.");
                }

                client.BaseAddress = new Uri(baseUrl);
            }*/
        }

        public async Task<List<Client>> GetClientsAsync()
        {
            try
            {
                var clients = await client.GetFromJsonAsync<List<Client>>("api/client");
                return clients ?? new List<Client>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
                return new List<Client>();
            }
        }

        public async Task<Client> AddClientAsync(Client clientObj)
        {
            var response = await client.PostAsJsonAsync("api/client", clientObj);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Client>();
        }

        public async Task UpdateClientAsync(Client clientObj)
        {
            var response = await client.PutAsJsonAsync($"api/client/{clientObj.ClientId}", clientObj);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteClientAsync(int clientId)
        {
            var response = await client.DeleteAsync($"api/client/{clientId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
