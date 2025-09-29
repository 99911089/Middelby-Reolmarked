using System;
using System.ComponentModel;         // Indeholder INotifyPropertyChanged
using System.Runtime.CompilerServices;

namespace Reolmarked.Model  // Namespace matcher projektets navn
{
    // Repræsenterer en lejeaftale (RentalAgreement)
    // Implementerer INotifyPropertyChanged så den kan bruges i WPF databinding
    public class RentalAgreement : INotifyPropertyChanged
    {
        // Felter (private variabler) til at gemme data
        private int _rentalAgreementId;
        private DateTime _startDate;
        private DateTime _endDate;
        private int _customerId;
        private int _rentalStatusId;
        private int _rackId; // Foreign key til Rack (Reol)

        // Egenskab for RentalAgreementId
        public int RentalAgreementId
        {
            get { return _rentalAgreementId; }                  // Getter returnerer værdien
            set                                                // Setter ændrer værdien
            {
                _rentalAgreementId = value;
                OnPropertyChanged("RentalAgreementId");         // Kalder event så UI opdateres
            }
        }

        // Egenskab for StartDate
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        // Egenskab for EndDate
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        // Foreign key til Customer (en kunde kan have mange RentalAgreements)
        public int CustomerId
        {
            get { return _customerId; }
            set
            {
                _customerId = value;
                OnPropertyChanged("CustomerId");
            }
        }

        // Foreign key til Rack (en reol kan have mange RentalAgreements)
        public int RackId
        {
            get { return _rackId; }
            set
            {
                _rackId = value;
                OnPropertyChanged("RackId");
            }
        }

        // Foreign key til RentalStatus (en status kan bruges af mange RentalAgreements)
        public int RentalStatusId
        {
            get { return _rentalStatusId; }
            set
            {
                _rentalStatusId = value;
                OnPropertyChanged("RentalStatusId");
            }
        }

        // Event der bruges til at meddele UI (fx WPF) at en værdi er ændret
        public event PropertyChangedEventHandler PropertyChanged;

        // Hjælpe-metode til at kalde eventen ovenfor
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) // Tjek at der er nogen der "lytter"
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        
    }
}

