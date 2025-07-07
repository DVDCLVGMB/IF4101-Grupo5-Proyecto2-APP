using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Steady_Management_App.DTOs;
using Steady_Management_App.Services;
using static System.Net.WebRequestMethods;

public class OrderApiService
{
    private readonly HttpClient _httpClient;

    public OrderApiService()
    {
        _httpClient = HttpClientProvider.Client; // <-- usa el que tiene el token
    }

    public async Task<string> CreateOrderAsync(OrderDTO order)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Order", order);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error del servidor: {response.StatusCode} - {responseContent}");
        }

        return responseContent;
    }

    public async Task<List<OrderDTO>> GetOrdersAsync()
    {
        // GetFromJsonAsync maneja el GET + deserialización
        var list = await _httpClient.GetFromJsonAsync<List<OrderDTO>>("api/Order");
        return list ?? new List<OrderDTO>();
    }

    // —————————————— UPDATE ——————————————
    public async Task UpdateOrderAsync(int id, OrderRequestDto rq)
    {
        var resp = await _httpClient.PutAsJsonAsync($"api/Order/{id}", rq);

        if (!resp.IsSuccessStatusCode)
        {
            // Lee aquí el mensaje de error que te envía el backend
            var errorDetail = await resp.Content.ReadAsStringAsync();
            throw new Exception($"Error {resp.StatusCode}: {errorDetail}");
        }
    }


    // —————————————— DELETE ——————————————
    public async Task DeleteOrderAsync(int id)
    {
        var resp = await _httpClient.DeleteAsync($"api/Order/{id}");
        resp.EnsureSuccessStatusCode();
    }

    public Task<OrderResponseDto?> GetOrderAsync(int id) =>
        _httpClient.GetFromJsonAsync<OrderResponseDto>($"api/Order/{id}");
}

