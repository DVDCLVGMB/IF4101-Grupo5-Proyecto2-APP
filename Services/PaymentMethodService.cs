using System.Net.Http;
using System.Net.Http.Json;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public class PaymentMethodService
    {
        private readonly HttpClient _http;

        public PaymentMethodService()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7284/")
            };
        }

        public async Task<string?> GetNameByIdAsync(int id)
        {
            return await _http.GetStringAsync($"api/payment/methods?methodId={id}");
        }
    }
}
