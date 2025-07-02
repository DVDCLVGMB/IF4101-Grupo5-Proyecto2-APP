using Steady_Management_App.Models;
using Steady_Management_App.Services;
using Steady_Management_App.ViewModels;
using Steady_Management.WPF.Views;
using System.Windows;
using System.Windows.Controls;

namespace Steady_Management_App.Views
{
    public partial class DepartmentFormUcView : UserControl
    {
        private readonly DepartmentService _departmentService = new();

        public DepartmentFormUcView()
        {
            InitializeComponent();
        }

        private bool ShowDeleteConfirmation()
        {
            var dialog = new DeleteConfirmationDialog();
            dialog.Owner = Window.GetWindow(this);
            dialog.ShowDialog();
            return dialog.DeleteConfirmed;
        }

        private async void DeleteDepartment_Click(object sender, RoutedEventArgs e)
        {
            var department = (Department)((FrameworkElement)sender).Tag;

            if (ShowDeleteConfirmation())
            {
                await _departmentService.DeleteDepartmentAsync(department.DeptId);

                // Recargar departamentos desde el ViewModel
                if (DataContext is DepartmentFormViewModel vm)
                {
                    await vm.LoadDepartmentsAsync();
                }
            }
        }
    }
}
