using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Steady_Management_App.DTOs;

namespace Steady_Management_App.Services
{
    public class InventoryService
    {
        private static readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7284/")
        };

        // GET: api/inventory
        public async Task<List<InventoryDTO>> GetInventoriesAsync()
        {
            var resp = await _http.GetAsync("api/inventory");
            var json = await resp.Content.ReadAsStringAsync();
            MessageBox.Show(json, "JSON en crudo");
            return JsonSerializer.Deserialize<List<InventoryDTO>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<InventoryDTO>();
        }


        // POST: api/inventory
        public async Task<bool> CreateInventoryAsync(InventoryDTO dto)
        {
            try
            {
                var resp = await _http.PostAsJsonAsync("api/inventory", dto);

                if (!resp.IsSuccessStatusCode)
                {
                    // Leemos el cuerpo de la respuesta para ver el mensaje de error del backend
                    var body = await resp.Content.ReadAsStringAsync();
                    throw new InvalidOperationException(
                        $"Error {resp.StatusCode}: {body}");
                }

                return true;
            }
            catch (Exception ex)
            {
                // re-lanzamos la excepción con toda la info
                throw new ApplicationException("No se pudo crear el inventario.", ex);
            }
        }


        // PUT: api/inventory/{id}
        public async Task<bool> UpdateInventoryAsync(InventoryDTO dto)
        {
            try
            {
                var resp = await _http.PutAsJsonAsync(
                    $"api/inventory/{dto.InventoryId}", dto);
                return resp.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // DELETE: api/inventory/{id}
        public async Task<bool> DeleteInventoryAsync(int inventoryId)
        {
            try
            {
                var resp = await _http.DeleteAsync($"api/inventory/{inventoryId}");
                return resp.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// GET: api/inventory/{id}
        /// Devuelve el inventario o null si no existe
        /// </summary>
        public async Task<InventoryDTO?> GetByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<InventoryDTO>($"api/inventory/{id}");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}
