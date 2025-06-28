using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Steady_Management.Domain;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public class RoleService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string baseUrl = "https://localhost:7284/";


        public RoleService()
        {
            if (client.BaseAddress == null)
            {
                if (string.IsNullOrEmpty(baseUrl))
                    throw new InvalidOperationException("La URL base de la API no está configurada.");

                client.BaseAddress = new Uri("https://localhost:7284/"); // o baseUrl
                client.GetFromJsonAsync<List<Role>>("api/roles");
            }
        }

        // GET: api/roles
        public async Task<List<Role>> GetRolesAsync()
        {
            try
            {
                var list = await client.GetFromJsonAsync<List<Role>>("api/roles");
                return list ?? new List<Role>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[RoleService] Error GET all: {ex.Message}");
                return new List<Role>();
            }
        }

        // GET: api/roles/{id}
        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            try
            {
                return await client.GetFromJsonAsync<Role>($"api/roles/{id}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[RoleService] Error GET id={id}: {ex.Message}");
                return null;
            }
        }

        // POST: api/roles
        public async Task<Role?> AddRoleAsync(string roleName)
        {
            var newRole = new Role { RoleName = roleName };
            var response = await client.PostAsJsonAsync("api/roles", newRole);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Role>();
        }

        // PUT: api/roles/{id}
        public async Task<bool> UpdateRoleAsync(Role r)
        {
            var response = await client.PutAsJsonAsync($"api/roles/{r.RoleId}", r);
            if (!response.IsSuccessStatusCode)
                Console.WriteLine($"Error PUT id={r.RoleId}: {response.StatusCode}");
            return response.IsSuccessStatusCode;
        }

        // DELETE: api/roles/{id}
        public async Task<bool> DeleteRoleAsync(int id)
        {
            var response = await client.DeleteAsync($"api/roles/{id}");
            if (!response.IsSuccessStatusCode)
                Console.WriteLine($"Error DELETE id={id}: {response.StatusCode}");
            return response.IsSuccessStatusCode;
        }
    }
}
