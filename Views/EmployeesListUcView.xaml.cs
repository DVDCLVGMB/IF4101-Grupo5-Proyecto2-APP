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

                emp.DepartmentName = dept?.DeptName ?? "Desconocido";
                emp.RoleName = role?.RoleName ?? "Desconocido";
            }

            // CAMBIO: asignar al ListView en lugar del DataGrid
            EmployeesListView.ItemsSource = employees;
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var form = new EmployeeForm();
            bool? result = form.ShowDialog();

            if (result == true)
                LoadEmployees();
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = (Employee)((FrameworkElement)sender).DataContext;
            var form = new EmployeeForm(employee.EmployeeId);
            bool? result = form.ShowDialog();

            if (result == true)
                LoadEmployees();
        }

        private async void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = (Employee)((FrameworkElement)sender).DataContext;

            var confirmDialog = new DeleteConfirmationDialog();
            confirmDialog.Owner = Window.GetWindow(this);
            confirmDialog.ShowDialog();

            if (confirmDialog.DeleteConfirmed)
            {
                await _employeeService.DeleteEmployeeAsync(employee.EmployeeId);
                LoadEmployees();
            }
        }

        // Opcional: si no vas a usarlo, este método se puede eliminar
        private void EmployeesListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var gridView = EmployeesListView.View as GridView;
            if (gridView == null || gridView.Columns.Count == 0) return;

            double totalWidth = EmployeesListView.ActualWidth;

            // Resta un poco para evitar desbordes visuales por el scroll
            double availableWidth = totalWidth - 35;

            // Número total de columnas
            int columnCount = gridView.Columns.Count;

            // Asignar mismo ancho a cada columna
            double columnWidth = availableWidth / columnCount;

            foreach (var column in gridView.Columns)
            {
                column.Width = columnWidth;
            }
        }

    }
}
