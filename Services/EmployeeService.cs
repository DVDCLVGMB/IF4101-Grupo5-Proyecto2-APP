using System.Net.Http;
using System.Net.Http.Json;
using Steady_Management.Domain;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://localhost:7284/api/";

        public EmployeeService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Employee>>(baseUrl + "employees");
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Employee>(baseUrl + $"employees/{id}");
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Department>>(baseUrl + "departments");
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Role>>(baseUrl + "roles");
        }

        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            var response = await _httpClient.PostAsJsonAsync(baseUrl + "employees", employee);
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            await _httpClient.PutAsJsonAsync(baseUrl + $"employees/{employee.EmployeeId}", employee);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            await _httpClient.DeleteAsync(baseUrl + $"employees/{id}");
        }
    }
}
