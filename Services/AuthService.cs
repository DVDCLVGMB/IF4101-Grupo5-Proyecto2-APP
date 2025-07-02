using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Steady_Management_App.DTOs;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;
        private readonly User _currentUser = new();

        private string? _jwt;   // token guardado en memoria

        /// <param name="baseUrl">URL base del backend (sin slash final), ej.: https://localhost:7284</param>
        public AuthService(string baseUrl, HttpClient? httpClient = null)
        {
            _baseUrl = baseUrl.TrimEnd('/');
            _http = httpClient ?? new HttpClient();
        }

        /// <summary>
        /// Envía credenciales al backend y, si son válidas,
        /// guarda el token + datos del usuario en memoria.
        /// </summary>
        public async Task<User> LoginAsync(LoginRequestDTO request)
        {
            var url = $"{_baseUrl}/api/Login";
            var payload = JsonSerializer.Serialize(request);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            using var resp = await _http.PostAsync(url, content);

            if (!resp.IsSuccessStatusCode)
            {
                var err = await resp.Content.ReadAsStringAsync();
                throw new InvalidOperationException(
                    $"Login falló ({(int)resp.StatusCode}): {err}");
            }

            var json = await resp.Content.ReadAsStringAsync();
            var loginDto = JsonSerializer.Deserialize<LoginResponseDTO>(json,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                           ?? throw new InvalidOperationException("Respuesta vacía del backend.");

            // Guardar token y encabezado Authorization para peticiones futuras
            _jwt = loginDto.Token;
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _jwt);

            // Actualizar modelo de usuario
            HttpClientProvider.Configure(_jwt);
            _currentUser.SetSession(loginDto.UserId, loginDto.Username, loginDto.RoleId);
            return _currentUser;
        }

        /// <summary>
        /// Limpia token, cabeceras y modelo de usuario.
        /// </summary>
        public void Logout()
        {
            _jwt = null;
            _http.DefaultRequestHeaders.Authorization = null;
            _currentUser.ClearSession();
            HttpClientProvider.Clear();
        }

        /// <summary>
        /// Acceso de solo lectura a la sesión actual.
        /// </summary>
        public User CurrentUser => _currentUser;
    }
}
