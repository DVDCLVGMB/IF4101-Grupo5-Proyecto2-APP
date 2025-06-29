using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public class CategoryService : ICategoryService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string baseUrl = "https://localhost:7284/";

        public CategoryService()
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri(baseUrl);
                // opcional: “calentar” la colección
                _ = client.GetFromJsonAsync<List<Category>>("api/category");
            }
        }

        /// <summary>GET: api/categories</summary>
        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                var list = await client.GetFromJsonAsync<List<Category>>("api/category");
                return list ?? new List<Category>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[CategoryService] Error GET all: {ex.Message}");
                return new List<Category>();
            }
        }
    }
}
