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
using System.Windows.Shapes;

namespace Steady_Management_App
{
    /// <summary>
    /// Lógica de interacción para MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ConsultarClientes_Click(object sender, RoutedEventArgs e)
        {
            //MainContent.Content = new ClientesListForm();
        }

        private void ConsultarOrdenes_Click(object sender, RoutedEventArgs e)
        {
            //MainContent.Content = new OrdenesListForm();
        }
    }
}
