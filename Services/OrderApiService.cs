using Steady_Management_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management_App.Services
{
    public class OrderApiService
    {
        private readonly HttpClient _httpClient;

        public OrderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateOrderAsync(OrderDTO orderDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/order", orderDto);
            return response.IsSuccessStatusCode;
        }
    }
}
