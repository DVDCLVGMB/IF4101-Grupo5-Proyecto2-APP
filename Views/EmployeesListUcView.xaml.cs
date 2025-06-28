using Steady_Management.WPF.Views;
using Steady_Management_App.Models;
using Steady_Management_App.Services;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Steady_Management_App.Views
{
    public partial class EmployeesListUcView : UserControl
    {
        private readonly EmployeeService _employeeService = new();
        private readonly DepartmentService _departmentService = new();
        private readonly RoleService _roleService = new();

        public EmployeesListUcView()
        {
            InitializeComponent();
            LoadEmployees();
        }

        public async void LoadEmployees()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            var departments = await _departmentService.GetDepartmentsAsync();
            var roles = await _roleService.GetRolesAsync();

            foreach (var emp in employees)
            {
                var dept = departments.FirstOrDefault(d => d.DeptId == emp.DeptId);
                var role = roles.FirstOrDefault(r => r.RoleId == emp.RoleId);

                emp.DepartmentName = dept?.deptName ?? "Desconocido";
                emp.RoleName = role?.RoleName ?? "Desconocido";
            }

            EmployeesDataGrid.ItemsSource = employees;
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var form = new EmployeeForm();
            form.Show(); // ❗ ShowDialog ya no se usa en UserControl, debe manejarse desde MainWindow si se desea una ventana emergente
            LoadEmployees();
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = (Employee)((FrameworkElement)sender).DataContext;
            var form = new EmployeeForm(employee.EmployeeId);
            form.Show(); // ❗ Igual aquí
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
