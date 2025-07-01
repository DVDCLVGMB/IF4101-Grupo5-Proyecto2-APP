using System.Windows;

namespace Steady_Management.WPF.Views
{
    public partial class DeleteConfirmationDialog : Window
    {
        public bool DeleteConfirmed { get; private set; }

        public DeleteConfirmationDialog()
        {
            InitializeComponent();
            DeleteConfirmed = false;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteConfirmed = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteConfirmed = false;
            this.Close();
        }
    }
}