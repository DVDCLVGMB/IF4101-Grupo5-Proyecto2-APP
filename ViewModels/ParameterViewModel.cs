using System.Threading.Tasks;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Steady_Management_App.Models;
using Steady_Management_App.Services;
using Steady_Management_App.Services.Interfaces;

namespace Steady_Management_App.ViewModels
{
    public partial class ParameterViewModel : ObservableObject
    {
        private readonly IParameterService _parameterService;

        [ObservableProperty]
        private Parameter _parameter = new Parameter();

        [ObservableProperty]
        private string _statusMessage;

        [ObservableProperty]
        private SolidColorBrush _statusMessageColor;

        public ParameterViewModel(IParameterService parameterService)
        {
            _parameterService = parameterService;

            LoadParameterCommand = new AsyncRelayCommand(async () => await LoadParameterAsync());
            SaveParametersCommand = new AsyncRelayCommand(async () => await SaveParametersAsync());

            _ = LoadParameterAsync();
        }

        public IAsyncRelayCommand LoadParameterCommand { get; }
        public IAsyncRelayCommand SaveParametersCommand { get; }

        private async Task LoadParameterAsync()
        {
            try
            {
                Parameter = await _parameterService.GetParameterAsync() ?? new Parameter();
                Parameter.NumeroFactura = 2;
                StatusMessage = "Parámetros cargados.";
                StatusMessageColor = new SolidColorBrush(Colors.DarkGreen);
            }
            catch (System.Exception ex)
            {
                StatusMessage = $"Error al cargar parámetros: {ex.Message}";
                StatusMessageColor = new SolidColorBrush(Colors.Red);
            }
        }

        private async Task SaveParametersAsync()
        {
            try
            {
                Parameter.NumeroFactura = 2;
                bool result = await _parameterService.UpdateParameterAsync(Parameter);

                if (result)
                {
                    StatusMessage = "Parámetros actualizados correctamente.";
                    StatusMessageColor = new SolidColorBrush(Colors.DarkGreen);
                }
                else
                {
                    StatusMessage = "Error al actualizar los parámetros.";
                    StatusMessageColor = new SolidColorBrush(Colors.Red);
                }
            }
            catch (System.Exception ex)
            {
                StatusMessage = $"Error al guardar parámetros: {ex.Message}";
                StatusMessageColor = new SolidColorBrush(Colors.Red);
            }
        }
    }
}