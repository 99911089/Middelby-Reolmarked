using System.ComponentModel;

namespace Reolmarked.Model
{
    public class Customer : INotifyPropertyChanged
    {
        private int _customerId;
        private string _customerName;
        private string _customerEmail;
        private string _customerPhone;

        // Tom konstruktør
        public Customer() { }

        // Konstruktør med parametre
        public Customer(string name, string email, string phone)
        {
            _customerName = name;
            _customerEmail = email;
            _customerPhone = phone;
        }

        // Id
        public int CustomerId
        {
            get => _customerId;
            set
            {
                if (_customerId != value)
                {
                    _customerId = value;
                    OnPropertyChanged(nameof(CustomerId));
                }
            }
        }

        // Navn
        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (_customerName != value)
                {
                    _customerName = value;
                    OnPropertyChanged(nameof(CustomerName));
                }
            }
        }

        // Email
        public string CustomerEmail
        {
            get => _customerEmail;
            set
            {
                if (_customerEmail != value)
                {
                    _customerEmail = value;
                    OnPropertyChanged(nameof(CustomerEmail));
                }
            }
        }

        // Telefon
        public string CustomerPhone
        {
            get => _customerPhone;
            set
            {
                if (_customerPhone != value)
                {
                    _customerPhone = value;
                    OnPropertyChanged(nameof(CustomerPhone));
                }
            }
        }

        // ToString til visning
        public override string ToString()
        {
            return $"{CustomerName} - {CustomerEmail} - {CustomerPhone}";
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
