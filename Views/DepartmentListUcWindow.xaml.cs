using Steady_Management_App.Models;
using Steady_Management_App.Models.Steady_Management_App.Models;
using System.Windows;

namespace Steady_Management_App.Views
{
    public partial class DepartmentListUcWindow : Window
    {
        public DepartmentListUcWindow(Department dept)
        {
            InitializeComponent();
            DataContext = dept;   // el ViewModel ya pasa la instancia
            AplicarRestriccionesPorRol();
        }


        private void AplicarRestriccionesPorRol()
        {
            int roleId = UserSession.RoleId;

            if (roleId == 21)
            {
                AgregarButton.Visibility = Visibility.Collapsed;
                EditarButton.Visibility = Visibility.Collapsed;
            }
        }



        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not Department d || string.IsNullOrWhiteSpace(d.DeptName))
            {
                MessageBox.Show("Por favor ingrese un nombre válido.",
                                "Validación",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;  // cierra la ventana y deja continuar al ViewModel
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // simplemente cierra sin guardar
        }
    }
}

