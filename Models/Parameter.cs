using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Steady_Management_App.Models
{
    public class Parameter : INotifyPropertyChanged
    {
        private int _numeroFactura = 2;
        public int NumeroFactura
        {
            get => _numeroFactura;
            set { _numeroFactura = value; OnPropertyChanged(); }
        }

        private string _nombreEmpresa;
        public string NombreEmpresa
        {
            get => _nombreEmpresa;
            set { _nombreEmpresa = value; OnPropertyChanged(); }
        }

        private string _telefonoEmpresa;
        public string TelefonoEmpresa
        {
            get => _telefonoEmpresa;
            set { _telefonoEmpresa = value; OnPropertyChanged(); }
        }

        private string _correoEmpresa;
        public string CorreoEmpresa
        {
            get => _correoEmpresa;
            set { _correoEmpresa = value; OnPropertyChanged(); }
        }

        private string _tipoPago;
        public string TipoPago
        {
            get => _tipoPago;
            set { _tipoPago = value; OnPropertyChanged(); }
        }

        private string _cedulaJuridica;
        public string CedulaJuridica
        {
            get => _cedulaJuridica;
            set { _cedulaJuridica = value; OnPropertyChanged(); }
        }

        public Parameter()
        {
            NumeroFactura = 2;
        }

        public Parameter(string nombreEmpresa, string telefonoEmpresa, string correoEmpresa, string tipoPago, string cedulaJuridica)
        {
            _nombreEmpresa = nombreEmpresa;
            _telefonoEmpresa = telefonoEmpresa;
            _correoEmpresa = correoEmpresa;
            _tipoPago = tipoPago;
            _cedulaJuridica = cedulaJuridica;
            NumeroFactura = 2;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}