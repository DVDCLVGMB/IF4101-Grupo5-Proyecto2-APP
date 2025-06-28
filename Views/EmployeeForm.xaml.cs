
using Steady_Management_App.Services;
using Steady_Management_App.Models;
using System.Windows;

namespace Steady_Management.WPF.Views
{
    public partial class EmployeeForm : Window
    {
        private readonly EmployeeService _employeeService = new();
        private readonly int? employeeId;

        public EmployeeForm(int? id = null)
        {
            InitializeComponent();
            employeeId = id;
            LoadCombos();

            if (employeeId.HasValue)
                LoadEmployee(employeeId.Value);
        }

        private async void LoadCombos()
        {
            DeptComboBox.ItemsSource = await _employeeService.GetDepartmentsAsync();
            RoleComboBox.ItemsSource = await _employeeService.GetRolesAsync();
        }

        private async void LoadEmployee(int id)
        {
            var emp = await _employeeService.GetEmployeeByIdAsync(id);
            NameTextBox.Text = emp.EmployeeName;
            SurnameTextBox.Text = emp.EmployeeSurname;
            ExtensionTextBox.Text = emp.Extension;
            PhoneTextBox.Text = emp.WorkPhoneNumber;
            DeptComboBox.SelectedValue = emp.DeptId;
            RoleComboBox.SelectedValue = emp.RoleId;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Visibility = Visibility.Collapsed;

            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(SurnameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ExtensionTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneTextBox.Text) ||
                DeptComboBox.SelectedValue == null ||
                RoleComboBox.SelectedValue == null)
            {
                ErrorText.Text = "Por favor, complete todos los campos.";
                ErrorText.Visibility = Visibility.Visible;
                return;
            }

            var emp = new Employee
            {
                EmployeeId = employeeId ?? 0,
                EmployeeName = NameTextBox.Text.Trim(),
                EmployeeSurname = SurnameTextBox.Text.Trim(),
                Extension = ExtensionTextBox.Text.Trim(),
                WorkPhoneNumber = PhoneTextBox.Text.Trim(),
                DeptId = (int)DeptComboBox.SelectedValue,
                RoleId = (int)RoleComboBox.SelectedValue
            };

            try
            {
                if (employeeId.HasValue)
                    await _employeeService.UpdateEmployeeAsync(emp);
                else
                    await _employeeService.CreateEmployeeAsync(emp);

                this.DialogResult = true;
                this.Close();
            }
            catch
            {
                ErrorText.Text = "Error al guardar. Intente nuevamente.";
                ErrorText.Visibility = Visibility.Visible;
            }
        }
    }
}
