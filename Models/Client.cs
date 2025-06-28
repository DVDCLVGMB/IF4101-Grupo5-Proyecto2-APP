using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Steady_Management_App.Models
{
    public class Client : INotifyPropertyChanged
    {
        private int _clientId;
        public int ClientId
        {
            get => _clientId;
            set { _clientId = value; OnPropertyChanged(); }
        }

        private int _cityId;
        public int CityId
        {
            get => _cityId;
            set { _cityId = value; OnPropertyChanged(); }
        }

        private string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set { _companyName = value; OnPropertyChanged(); }
        }

        private string _contactName;
        public string ContactName
        {
            get => _contactName;
            set { _contactName = value; OnPropertyChanged(); }
        }

        private string _contactSurname;
        public string ContactSurname
        {
            get => _contactSurname;
            set { _contactSurname = value; OnPropertyChanged(); }
        }

        private string _contactRank;
        public string ContactRank
        {
            get => _contactRank;
            set { _contactRank = value; OnPropertyChanged(); }
        }

        private string _clientAddress;
        public string ClientAddress
        {
            get => _clientAddress;
            set { _clientAddress = value; OnPropertyChanged(); }
        }

        private string _clientPhoneNumber;
        public string ClientPhoneNumber
        {
            get => _clientPhoneNumber;
            set { _clientPhoneNumber = value; OnPropertyChanged(); }
        }

        private string _clientFaxNumber;
        public string ClientFaxNumber
        {
            get => _clientFaxNumber;
            set { _clientFaxNumber = value; OnPropertyChanged(); }
        }

        private string _clientPostalCode;
        public string ClientPostalCode
        {
            get => _clientPostalCode;
            set { _clientPostalCode = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
