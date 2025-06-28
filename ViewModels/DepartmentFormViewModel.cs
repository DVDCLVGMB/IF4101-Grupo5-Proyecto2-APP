using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Steady_Management_App.ViewModels
{
    public partial class DepartmentFormViewModel : ObservableObject
    {
        [ObservableProperty]
        private string departmentName;

        public IRelayCommand SaveCommand { get; }

        public DepartmentFormViewModel()
        {
         //   SaveCommand = new RelayCommand(Save);
        }

        private void Save()
        {
            // Aquí haces la lógica de guardar
            MessageBox.Show($"Guardado: {DepartmentName}");
        }
    }

}
