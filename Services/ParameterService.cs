using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Steady_Management_App.Models;
using Steady_Management_App.Services.Interfaces;

namespace Steady_Management_App.Services
{
    public class ParameterService : IParameterService
    {
        private static readonly HttpClient _client = HttpClientProvider.Client;

        public ParameterService()
        {/*
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7284/")
            };*/
        }

        public async Task<Parameter> GetParameterAsync()
        {
            var parameter = await _client.GetFromJsonAsync<Parameter>("api/parameter");
            return parameter;
        }

        public async Task<bool> UpdateParameterAsync(Parameter parameter)
        {
            var response = await _client.PutAsJsonAsync("api/parameter", parameter);
            return response.IsSuccessStatusCode;
        }
    }
}
