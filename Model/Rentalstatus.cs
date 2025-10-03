using System.ComponentModel;   // Indeholder INotifyPropertyChanged interfacet

namespace Reolmarked.Model      // Sørg for at namespace passer til dit projekt
{
    // Repræsenterer en status for en lejeaftale (fx "Aktiv", "Afsluttet", "Annulleret")
    // Implementerer INotifyPropertyChanged så ændringer kan opdateres i UI
    public class RentalStatus : INotifyPropertyChanged
    {
        // Privat felt til ID
        private int _rentalStatusId;

        // Privat felt til navn
        private string _rentalStatusName;

        // Offentlig property for ID
        public int RentalStatusId
        {
            get { return _rentalStatusId; }
            set
            {
                if (_rentalStatusId != value)       // Kun hvis værdien ændres
                {
                    _rentalStatusId = value;
                    OnPropertyChanged("RentalStatusId"); // Giv besked til UI
                }
            }
        }

        // Offentlig property for navn
        public string RentalStatusName
        {
            get { return _rentalStatusName; }
            set
            {
                if (_rentalStatusName != value)
                {
                    _rentalStatusName = value;
                    OnPropertyChanged("RentalStatusName"); // Giv besked til UI
                }
            }
        }

        // Event som UI kan "lytte" på
        public event PropertyChangedEventHandler PropertyChanged;

        // Hjælpe-metode til at fyre eventen af
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // ToString – så den vises pænt i fx en ListBox
        public override string ToString()
        {
            return $"{RentalStatusName}";
        }
    }
}
