using System.Windows;

namespace Steady_Management.WPF.Views
{
    public partial class ValidationDialog : Window
    {
        public ValidationDialog()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}