using Steady_Management_App.Services;
using Steady_Management_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Steady_Management_App.Views
{
    /// <summary>
    /// Lógica de interacción para ParameterUcView.xaml
    /// </summary>
    public partial class ParameterUcView : UserControl
    {
        public ParameterUcView()
        {
            InitializeComponent();
            DataContext = new ParameterViewModel(new ParameterService()); // 👈 Asegúrate de esto
        }
    }
}