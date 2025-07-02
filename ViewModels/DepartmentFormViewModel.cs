using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Steady_Management_App.Models;
using Steady_Management_App.Services;
using Steady_Management_App.Views;

namespace Steady_Management_App.ViewModels
{
    public partial class DepartmentFormViewModel : ObservableObject
    {
        private readonly DepartmentService _apiService;

        [ObservableProperty] private string searchTerm = string.Empty;
        [ObservableProperty] private bool isLoading;
        [ObservableProperty] private Department? selectedDepartment;

        public ObservableCollection<Department> Departments { get; } = new();
        public ICollectionView DepartmentsView { get; }

        public IAsyncRelayCommand AddCommand { get; }
        public IAsyncRelayCommand<Department> EditCommand { get; }
        public IAsyncRelayCommand<Department> DeleteCommand { get; }
        
        public DepartmentFormViewModel()
        {
            _apiService = new DepartmentService();

            // Vista filtrable
            DepartmentsView = CollectionViewSource.GetDefaultView(Departments);
            DepartmentsView.Filter = FilterDepartments;

            AddCommand = new AsyncRelayCommand(AddDepartment);
            EditCommand = new AsyncRelayCommand<Department>(EditDepartment);
            DeleteCommand = new AsyncRelayCommand<Department>(DeleteDepartment);

            _ = LoadDepartmentsAsync();
        }

        /*-----  Filtro dinámico  -----*/
        partial void OnSearchTermChanged(string value) => DepartmentsView.Refresh();

        private bool FilterDepartments(object obj)
        {
            if (obj is not Department dept) return false;
            if (string.IsNullOrWhiteSpace(SearchTerm)) return true;
            return dept.DeptName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
        }

        // -------------------- CRUD --------------------
        public async Task LoadDepartmentsAsync()
        {
            IsLoading = true;
            var list = await _apiService.GetDepartmentsAsync();
            Departments.Clear();
            foreach (var d in list) Departments.Add(d);
            IsLoading = false;
        }

        private async Task AddDepartment()
        {
            var nuevo = new Department();
            var win = new DepartmentListUcWindow(nuevo);   // tu ventana modal

            if (win.ShowDialog() == true)
            {
                try
                {
                    var creado = await _apiService.AddDepartmentAsync(nuevo.DeptName);
                    if (creado != null) Departments.Add(creado);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al crear: {ex.Message}");
                }
            }
        }

        private async Task EditDepartment(Department? dept)
        {
            if (dept == null) return;

            // Copia editable
            var editable = new Department { DeptId = dept.DeptId, DeptName = dept.DeptName };
            var win = new DepartmentListUcWindow(editable);

            if (win.ShowDialog() == true)
            {
                try
                {
                    await _apiService.UpdateDepartmentAsync(editable);

                    // reflejar cambios
                    var idx = Departments.IndexOf(dept);
                    if (idx >= 0) Departments[idx] = editable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar: {ex.Message}");
                }
            }
        }

        private async Task DeleteDepartment(Department? dept)
        {
            if (dept == null) return;

            var ok = MessageBox.Show($"¿Eliminar \"{dept.DeptName}\"?",
                                     "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (ok != MessageBoxResult.Yes) return;

            try
            {
                await _apiService.DeleteDepartmentAsync(dept.DeptId);
                Departments.Remove(dept);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}");
            }
        }
    }
}


