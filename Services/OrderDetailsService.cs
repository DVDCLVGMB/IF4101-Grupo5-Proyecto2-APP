using System.Net.Http;
using System.Net.Http.Json;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public class OrderDetailService
    {
        private readonly HttpClient _http;

        public OrderDetailService()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7284/")
            };
        }

        public async Task<List<OrderDetail>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<OrderDetail>>("api/orderDetails")
                   ?? new List<OrderDetail>();
        }
    }
}

