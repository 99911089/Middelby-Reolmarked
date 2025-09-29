using System.ComponentModel;

namespace Reolmarked.Model
{
    public class Rack : INotifyPropertyChanged
    {
        private int _rackId;
        private int _amountShelves;
        private bool _hangerBar;
        private bool _isOccupied;
        private string _productName;

        public int RackId
        {
            get { return _rackId; }
            set { _rackId = value; OnPropertyChanged("RackId"); }
        }

        public int AmountShelves
        {
            get { return _amountShelves; }
            set { _amountShelves = value; OnPropertyChanged("AmountShelves"); }
        }

        public bool HangerBar
        {
            get { return _hangerBar; }
            set { _hangerBar = value; OnPropertyChanged("HangerBar"); OnPropertyChanged("HangerBarText"); }
        }

        // Ny property til læsbar tekst
        public string HangerBarText
        {
            get { return HangerBar ? "Har bøjlestang" : "Ingen bøjlestang"; }
        }

        public bool IsOccupied
        {
            get { return _isOccupied; }
            set { _isOccupied = value; OnPropertyChanged("IsOccupied"); OnPropertyChanged("OccupiedText"); }
        }

        // Ny property til læsbar tekst for optaget/ledig
        public string OccupiedText
        {
            get { return IsOccupied ? "Optaget" : "Ledig"; }
        }

        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; OnPropertyChanged("ProductName"); }
        }

        // Speciel tekst til Vis bøjler
        public string ToHangerString()
        {
            if (IsOccupied)
                return $"Reol {RackId} - Har bøjlestang - {ProductName}";
            else
                return $"Reol {RackId} - Har bøjlestang - Ledig";
        }

        public override string ToString()
        {
            return IsOccupied
                ? $"Reol {RackId} - {ProductName}"
                : $"Reol {RackId} - Ledig";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
