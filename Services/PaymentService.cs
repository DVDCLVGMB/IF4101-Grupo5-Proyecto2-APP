using System.Net.Http;
using System.Net.Http.Json;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public class PaymentService
    {
        private readonly HttpClient _http;

        public PaymentService()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7284/")
            };
        }

        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            return await _http.GetFromJsonAsync<Payment>($"api/payment/order/{orderId}");
        }

        public async Task<bool> InsertPaymentAsync(Payment payment)
        {
            var response = await _http.PostAsJsonAsync("api/payment", payment);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteByOrderIdAsync(int orderId)
        {
            var response = await _http.DeleteAsync($"api/payment/order/{orderId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<string?> GetPaymentMethodNameAsync(int methodId)
        {
            return await _http.GetStringAsync($"api/payment/methods?methodId={methodId}");
        }
    }
}

