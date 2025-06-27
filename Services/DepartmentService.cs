using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Steady_Management_App.DTOs;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public class DepartmentService
    {
        private static readonly HttpClient client = new HttpClient();
        // La URL base ahora se lee desde el archivo de configuración global
        private readonly string baseUrl = AppConfig.GetApiBaseUrl();
    
        public DepartmentService() 
        {
            if (client.BaseAddress == null)
            {
                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new InvalidOperationException("La URL base de la API no está configurada en appsettings.json.");
                }
                client.BaseAddress = new Uri("https://localhost:7298/");
                client.GetFromJsonAsync<List<Department>>("api/departments");
            }
        }

        // -------------------  READ ALL  -------------------
        /// <summary>GET: api/departments</summary>
        public async Task<List<Department>> GetDepartmentsAsync()
        {
            try
            {
                var list = await client.GetFromJsonAsync<List<Department>>("api/departments");
                return list ?? new List<Department>();
            }
            catch (HttpRequestException ex)
            {
                // Log, Toast o MessageBox.Show según tu UI
                Console.WriteLine($"[DepartmentService] Error GET all: {ex.Message}");
                return new List<Department>();
            }
        }

        // -------------------  READ BY ID  -----------------
        /// <summary>GET: api/departments/{id}</summary>
        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            try
            {
                return await client.GetFromJsonAsync<Department>($"api/departments/{id}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[DepartmentService] Error GET id={id}: {ex.Message}");
                return null;
            }
        }

        // -------------------  CREATE  ---------------------
        /// <summary>POST: api/departments</summary>
        /// Variante A: enviando el mismo modelo Department (id = 0)
        public async Task<Department?> AddDepartmentAsync(string deptName)
        {
            var dto = new DepartmentDTO { DeptName = deptName };
            var response = await client.PostAsJsonAsync("api/departments", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Department>();
        }

        // -------------------  UPDATE  ---------------------
        /// <summary>PUT: api/departments/{id}</summary>
        public async Task<bool> UpdateDepartmentAsync(Department d)
        {
            var response = await client.PutAsJsonAsync($"api/departments/{d.deptId}", d);
            if (!response.IsSuccessStatusCode)
                Console.WriteLine($"Error PUT id={d.deptId}: {response.StatusCode}");
            return response.IsSuccessStatusCode;
        }

        // -------------------  DELETE  ---------------------
        /// <summary>DELETE: api/departments/{id}</summary>
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var response = await client.DeleteAsync($"api/departments/{id}");
            if (!response.IsSuccessStatusCode)
                Console.WriteLine($"Error DELETE id={id}: {response.StatusCode}");
            return response.IsSuccessStatusCode;
        }
    }
}
