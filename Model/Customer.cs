using System.ComponentModel;

namespace Reolmarked.Model
{
    // Klassen repræsenterer en kunde i systemet
    // Den implementerer INotifyPropertyChanged, så ændringer kan ses i UI
    public class Customer : INotifyPropertyChanged
    {
        // Felter til at gemme data internt
        private int _customerId;
        private string _customerName;
        private string _customerEmail;
        private string _customerPhone;

        // Tom konstruktør
        public Customer() { }

        // Konstruktør til at oprette ny kunde uden id
        public Customer(string name, string email, string phone)
        {
            _customerName = name;
            _customerEmail = email;
            _customerPhone = phone;
        }

        // Konstruktør til kunde med id
        public Customer(int id, string name, string email, string phone)
        {
            _customerId = id;
            _customerName = name;
            _customerEmail = email;
            _customerPhone = phone;
        }

        // KundeId – unikt nummer
        public int CustomerId
        {
            get { return _customerId; }
            set
            {
                if (_customerId != value)
                {
                    _customerId = value;
                    OnPropertyChanged("CustomerId");
                }
            }
        }

        // Kundens navn
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                if (_customerName != value)
                {
                    _customerName = value;
                    OnPropertyChanged("CustomerName");
                }
            }
        }

        // Kundens email
        public string CustomerEmail
        {
            get { return _customerEmail; }
            set
            {
                if (_customerEmail != value)
                {
                    _customerEmail = value;
                    OnPropertyChanged("CustomerEmail");
                }
            }
        }

        // Kundens telefonnummer
        public string CustomerPhone
        {
            get { return _customerPhone; }
            set
            {
                if (_customerPhone != value)
                {
                    _customerPhone = value;
                    OnPropertyChanged("CustomerPhone");
                }
            }
        }

        // ToString bruges til at vise kunden i en ListBox
        public override string ToString()
        {
            return CustomerName + " (" + CustomerEmail + ", " + CustomerPhone + ")";
        }

        // Event til INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // Hjælpe-metode til at sende besked om ændringer til UI
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
