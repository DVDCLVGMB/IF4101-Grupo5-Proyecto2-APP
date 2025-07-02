using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public class CityApiService
    {
        private static readonly HttpClient _client = new();
        private readonly string _baseUrl = AppConfig.GetApiBaseUrl();

        public CityApiService()
        {
            if (_client.BaseAddress == null)
            {
                if (string.IsNullOrEmpty(_baseUrl))
                    throw new InvalidOperationException("La URL base de la API no está configurada.");
                _client.BaseAddress = new Uri(_baseUrl);
            }
        }

        public async Task<List<City>> GetCitiesAsync()
            => await _client.GetFromJsonAsync<List<City>>("api/city")
               ?? new List<City>();

        public async Task<City> AddCityAsync(City city)
        {
            var resp = await _client.PostAsJsonAsync("api/city", city);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<City>();
        }

        public async Task UpdateCityAsync(City city)
        {
            var resp = await _client.PutAsJsonAsync($"api/city/{city.CityId}", city);
            resp.EnsureSuccessStatusCode();
        }

        public async Task<HttpResponseMessage> DeleteCityAsync(int id)
     => await _client.DeleteAsync($"api/city/{id}");


    }
}
