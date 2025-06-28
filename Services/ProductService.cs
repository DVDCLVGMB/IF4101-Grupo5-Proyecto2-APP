using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Steady_Management_App.Models;
using Steady_Management_App.DTOs;

namespace Steady_Management_App.Services
{
    public class ProductService
    {
        private static readonly HttpClient client = new HttpClient();
        // La URL base ahora se lee (idealmente) desde appsettings.json,
        private readonly string baseUrl = "https://localhost:7284/";

        public ProductService()
        {
            if (client.BaseAddress == null)
            {
                if (string.IsNullOrEmpty(baseUrl))
                    throw new InvalidOperationException(
                        "La URL base de la API no está configurada en appsettings.json.");
                client.BaseAddress = new Uri(baseUrl);
                // opcional: «calentar» la colección al arrancar
                _ = client.GetFromJsonAsync<List<Product>>("api/products");
            }
        }

        // ------------------- READ ALL -------------------
        /// <summary>GET: api/products</summary>
        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var list = await client.GetFromJsonAsync<List<Product>>("api/products");
                return list ?? new List<Product>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[ProductService] Error GET all: {ex.Message}");
                return new List<Product>();
            }
        }

        // ------------------- READ BY ID ------------------
        /// <summary>GET: api/products/{id}</summary>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                return await client.GetFromJsonAsync<Product>($"api/products/{id}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[ProductService] Error GET id={id}: {ex.Message}");
                return null;
            }
        }

        // ------------------- CREATE ----------------------
        /// <summary>POST: api/products</summary>
        public async Task<Product?> AddProductAsync(
            string productName,
            int categoryId,
            float price)
        {
            var dto = new ProductDTO
            {
                ProductName = productName,
                CategoryId = categoryId,
                Price = price
            };

            try
            {
                var response = await client.PostAsJsonAsync("api/products", dto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Product>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[ProductService] Error POST: {ex.Message}");
                return null;
            }
        }

        // ------------------- UPDATE ----------------------
        /// <summary>PUT: api/products/{id}</summary>
        public async Task<bool> UpdateProductAsync(Product p)
        {
            try
            {
                var response = await client.PutAsJsonAsync(
                    $"api/products/{p.ProductId}",
                    p);

                if (!response.IsSuccessStatusCode)
                    Console.WriteLine(
                        $"[ProductService] Error PUT id={p.ProductId}: {response.StatusCode}");

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(
                    $"[ProductService] Error PUT id={p.ProductId}: {ex.Message}");
                return false;
            }
        }

        // ------------------- DELETE ----------------------
        /// <summary>DELETE: api/products/{id}</summary>
        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var response = await client.DeleteAsync($"api/products/{id}");
                if (!response.IsSuccessStatusCode)
                    Console.WriteLine(
                        $"[ProductService] Error DELETE id={id}: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[ProductService] Error DELETE id={id}: {ex.Message}");
                return false;
            }
        }
    }
}
