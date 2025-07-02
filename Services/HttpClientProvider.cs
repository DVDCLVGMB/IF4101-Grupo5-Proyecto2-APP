using System.Net.Http;

namespace Steady_Management_App.Services
{
    public static class HttpClientProvider
    {
        public static HttpClient Client { get; private set; } = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7284/") // ← BaseAddress configurada
        };

        public static void Configure(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public static void Clear()
        {
            Client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
