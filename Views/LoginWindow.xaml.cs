using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using Steady_Management_App.DTOs;

namespace Steady_Management_App.Views
{
    public partial class LoginWindow : Window
    {
        private static readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7284/")
        };

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Visibility = Visibility.Collapsed;

            var request = new LoginRequestDTO
            {
                Username = UsernameBox.Text.Trim(),
                Password = PasswordBox.Password
            };

            HttpResponseMessage resp;
            try
            {
                resp = await _http.PostAsJsonAsync("api/Login", request);
            }
            catch
            {
                ErrorText.Text = "No se pudo conectar al servidor.";
                ErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (resp.IsSuccessStatusCode)
            {
                var result = await resp.Content.ReadFromJsonAsync<LoginResponseDTO>();
                Application.Current.Properties["UserRole"] = result!.RoleId;
                Application.Current.Properties["UserName"] = result.Username;
                DialogResult = true;
            }
            else if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ErrorText.Text = "Usuario o contraseña inválidos.";
                ErrorText.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorText.Text = $"Error de servidor: {(int)resp.StatusCode}.";
                ErrorText.Visibility = Visibility.Visible;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}