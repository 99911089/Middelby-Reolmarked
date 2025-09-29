using System.ComponentModel;   // Indeholder INotifyPropertyChanged interfacet

namespace Reolmarked.Model      // Namespace matcher projektets navn
{
    // Repræsenterer en status for en lejeaftale (fx "Aktiv", "Afsluttet", "Annulleret")
    // Implementerer INotifyPropertyChanged for at understøtte databinding i WPF
    public class RentalStatus : INotifyPropertyChanged
    {
        // Private felter til at gemme værdier
        private int _rentalStatusId;       // Unikt ID for lejestatus
        private string _rentalStatusName;  // Navn på lejestatus

        // Egenskab for RentalStatusId
        public int RentalStatusId
        {
            get { return _rentalStatusId; }                     // Returnerer værdien
            set
            {
                _rentalStatusId = value;                        // Sætter ny værdi
                OnPropertyChanged("RentalStatusId");            // Besked til UI om at værdien er ændret
            }
        }

        // Egenskab for RentalStatusName
        public string RentalStatusName
        {
            get { return _rentalStatusName; }
            set
            {
                _rentalStatusName = value;
                OnPropertyChanged("RentalStatusName");
            }
        }

        // Event som UI kan "lytte på" og reagere når en property ændres
        public event PropertyChangedEventHandler PropertyChanged;

        // Metode til at rejse PropertyChanged eventen manuelt
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)   // Tjekker at der er abonnenter
            {
                // Sender besked til UI: "property X er ændret"
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

