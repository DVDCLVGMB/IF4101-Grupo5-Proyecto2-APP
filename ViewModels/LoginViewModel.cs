using Steady_Management_App.DTOs;
using Steady_Management_App.Models;
using Steady_Management_App.Models.Steady_Management_App.Models;
using Steady_Management_App.Services;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Steady_Management_App.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username = string.Empty;
        private string _password = string.Empty;

        private readonly AuthService _authService;
        private readonly User _user;

        public event PropertyChangedEventHandler? PropertyChanged;

        public LoginViewModel()
        {
            _authService = new AuthService("https://localhost:7284");
            _user = _authService.CurrentUser;

            LoginCommand = new RelayCommand(async _ => await LoginAsync());
            LogoutCommand = new RelayCommand(_ => Logout());
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }

        public async Task LoginAsync()
        {
            try
            {
                var loginDto = new LoginRequestDTO
                {
                    Username = this.Username,
                    Password = this.Password
                };

                await _authService.LoginAsync(loginDto);

                var currentUser = _authService.CurrentUser;

                //  Validación por si acaso
                if (currentUser == null)
                    throw new Exception("No se pudo obtener el usuario desde el servidor.");

                UserSession.UserId = currentUser.UserId;
                UserSession.Username = currentUser.Username;
                UserSession.RoleId = currentUser.RoleId;


                MessageBox.Show($"Bienvenido {currentUser.Username}", "Login exitoso",
                    MessageBoxButton.OK, MessageBoxImage.Information);

               

                CloseLogin?.Invoke(); // Cierra la ventana de login

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de autenticación",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Logout()
        {
            _authService.Logout();
            MessageBox.Show("Sesión cerrada", "Logout", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Delegados para comunicar con la vista (LoginWindow)
        public Action? CloseLogin { get; set; }
    }
}
