using System.Windows;
using Steady_Management_App.ViewModels;

namespace Steady_Management_App.Views
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _vm;

        public LoginWindow()
        {
            InitializeComponent();

            _vm = new LoginViewModel();

            // Pasamos la contraseña manualmente desde el PasswordBox
            this.DataContext = _vm;

            // Asignamos el cierre desde el ViewModel
            _vm.CloseLogin = CloseLogin;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Password = PasswordBox.Password;
            await _vm.LoginAsync();
        }

        public void CloseLogin()
        {
            this.DialogResult = true;   // <-- indica al MainWindow que el login fue OK
            this.Close();
        }

    }
}
