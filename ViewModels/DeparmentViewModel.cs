using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Steady_Management_App.Models;
using Steady_Management_App.Services;
using Steady_Management_App.Views; 

namespace Steady_Management_App.ViewModels
{
    public class DepartmentViewModel : ObservableObject
    {
        private readonly DepartmentService _apiService;
        private ObservableCollection<Department> _departments;
        private Department _selectedDepartment;
        private bool _isLoading;

        public ObservableCollection<Department> Departments
        {
            get => _departments;
            set => SetProperty(ref _departments, value);
        }

        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set => SetProperty(ref _selectedDepartment, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public IAsyncRelayCommand AddCommand { get; }
        public IAsyncRelayCommand EditCommand { get; }
        public IAsyncRelayCommand DeleteCommand { get; }

        public DepartmentViewModel()
        {
            _apiService = new DepartmentService();
            Departments = new ObservableCollection<Department>();

            AddCommand = new AsyncRelayCommand(AddDepartment);
            EditCommand = new AsyncRelayCommand(EditDepartment);
            DeleteCommand = new AsyncRelayCommand(DeleteDepartment);

            _ = LoadDepartmentsAsync(); // async void-like para arrancar carga
        }

        private async Task LoadDepartmentsAsync()
        {
            IsLoading = true;
            var list = await _apiService.GetDepartmentsAsync();
            Departments.Clear();
            foreach (var d in list)
            {
                Departments.Add(d);
            }
            IsLoading = false;
        }

        private async Task AddDepartment()
        {
            var newDept = new Department();
            var form = new DepartmentFormWindow(newDept);

            if (form.ShowDialog() == true)
            {
                try
                {
                    var created = await _apiService.AddDepartmentAsync(newDept.deptName);
                    if (created != null)
                        Departments.Add(created);

                    MessageBox.Show("Departamento creado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al crear departamento: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task EditDepartment()
        {
            if (SelectedDepartment == null) return;

            // Copia de seguridad para editar
            var deptToEdit = new Department
            {
                DeptId = SelectedDepartment.DeptId,
                deptName = SelectedDepartment.deptName
            };

            var form = new DepartmentFormWindow(deptToEdit);
            if (form.ShowDialog() == true)
            {
                try
                {
                    await _apiService.UpdateDepartmentAsync(deptToEdit);
                    var index = Departments.IndexOf(SelectedDepartment);
                    if (index >= 0)
                        Departments[index] = deptToEdit;

                    MessageBox.Show("Departamento actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar departamento: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task DeleteDepartment()
        {
            if (SelectedDepartment == null) return;

            var confirm = MessageBox.Show($"¿Estás seguro de eliminar el departamento \"{SelectedDepartment.deptName}\"?",
                                          "Confirmar eliminación",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Warning);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiService.DeleteDepartmentAsync(SelectedDepartment.DeptId);
                    Departments.Remove(SelectedDepartment);

                    MessageBox.Show("Departamento eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar departamento: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
