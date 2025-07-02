using System.Buffers.Text;
using System.Net.Http;
using System.Net.Http.Json;
using Steady_Management.Domain;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public class EmployeeService
    {
        private static readonly HttpClient _httpClient = HttpClientProvider.Client; 
        //private readonly string baseUrl = "https://localhost:7284/api/";

        public EmployeeService()
        {
            //_httpClient = new HttpClient();
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Employee>>("api/employees")
                ?? new List<Employee>();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Employee>($"api/employees/{id}");

        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Department>>("api/departments")
                ?? new List<Department>();
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Role>>("api/roles") ?? new List<Role>();
        }

        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            var response = await _httpClient.PostAsJsonAsync("api/employees", employee);
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            await _httpClient.PutAsJsonAsync($"api/employees/{employee.EmployeeId}", employee);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/employees/{id}");
        }
    }
}
