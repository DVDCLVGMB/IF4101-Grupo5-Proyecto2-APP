
using Steady_Management_App.Services;
using Steady_Management_App.Models;
using System.Windows;

namespace Steady_Management.WPF.Views
{
    public partial class EmployeesView : Window
    {
        private readonly EmployeeService _employeeService = new();

        public EmployeesView()
        {
            InitializeComponent();
            LoadEmployees();
        }

      
        private readonly DepartmentService _departmentService = new();
        private readonly RoleService _roleService = new();

        private async void LoadEmployees()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            var departments = await _departmentService.GetDepartmentsAsync();
            var roles = await _roleService.GetRolesAsync();

            // Relacionar los nombres
            foreach (var emp in employees)
            {
                var dept = departments.FirstOrDefault(d => d.DeptId == emp.DeptId);
                var role = roles.FirstOrDefault(r => r.RoleId == emp.RoleId);

                emp.DepartmentName = dept?.DeptName ?? "Desconocido";
                emp.RoleName = role?.RoleName ?? "Desconocido";
            }

            EmployeesDataGrid.ItemsSource = employees;
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var form = new EmployeeForm();
            form.ShowDialog();
            LoadEmployees();
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = (Employee)((FrameworkElement)sender).DataContext;
            var form = new EmployeeForm(employee.EmployeeId);
            form.ShowDialog();
            LoadEmployees();
        }

        private async void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = (Employee)((FrameworkElement)sender).DataContext;
            if (MessageBox.Show("¿Desea eliminar este empleado?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _employeeService.DeleteEmployeeAsync(employee.EmployeeId);
                LoadEmployees();
            }
        }
    }
}
