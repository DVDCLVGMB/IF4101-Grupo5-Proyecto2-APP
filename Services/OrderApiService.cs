using System.Net.Http;
using System.Net.Http.Json;
using Steady_Management_App.Services;

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
}

