using Steady_Management.WPF.Views;
using Steady_Management_App.Models;
using Steady_Management_App.Services;
using Steady_Management_App.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Steady_Management_App.Views
{
    public partial class DepartmentFormUcView : UserControl
    {
        private readonly DepartmentService _departmentService = new();

        public DepartmentFormUcView()
        {
            InitializeComponent();
        }


        private void DepartmentFormUcView_Loaded(object sender, RoutedEventArgs e)
        {
            AplicarRestriccionesPorRol();
        }

        private void AplicarRestriccionesPorRol()
        {
            string rol = Application.Current.Properties["UserRole"]?.ToString() ?? "0";
            int.TryParse(rol, out int roleId);

            if (roleId == 21) // Empleado
            {
                // Ocultar botón agregar
                AgregarButton.Visibility = Visibility.Collapsed;

                // También ocultar las acciones (editar/eliminar) por columna
                // Recorremos visualmente el árbol para desactivar los botones
                if (this.FindName("ProductsListView") is ListView listView)
                {
                    foreach (var item in listView.Items)
                    {
                        var container = listView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                        if (container != null)
                        {
                            var editBtn = FindVisualChild<Button>(container, "EditarButton");
                            var delBtn = FindVisualChild<Button>(container, "EliminarButton");

                            if (editBtn != null) editBtn.Visibility = Visibility.Collapsed;
                            if (delBtn != null) delBtn.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        private static T? FindVisualChild<T>(DependencyObject parent, string name = "") where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild && (string.IsNullOrEmpty(name) || (child as FrameworkElement)?.Name == name))
                    return typedChild;

                var result = FindVisualChild<T>(child, name);
                if (result != null) return result;
            }

            return null;
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
