using Steady_Management_App.Models;
using Steady_Management_App.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Steady_Management_App.ViewModels
{
    public class EmployeeViewModel
    {
        private readonly EmployeeService _employeeService = new EmployeeService();

        public List<Employee> Employees { get; private set; } = new List<Employee>();
        public Employee SelectedEmployee { get; set; }
        public Employee CurrentEmployee { get; set; }

        public async Task LoadEmployees()
        {
            Employees = await _employeeService.GetEmployeesAsync(); // ✅ CORRECTO
        }

        public async Task<bool> SaveEmployee(Employee employee)
        {
            try
            {
                if (employee.EmployeeId == 0)
                {
                    await _employeeService.CreateEmployeeAsync(employee); // ✅ CORRECTO
                }
                else
                {
                    await _employeeService.UpdateEmployeeAsync(employee); // ✅ CORRECTO
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(id); // ✅ CORRECTO
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}