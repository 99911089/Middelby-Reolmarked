using Reolmarked.Model;
using Reolmarked.Repository.DbRepo;
using Reolmarked.View; 
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;  // Indeholder INotifyPropertyChanged interfacet
using System.Runtime.CompilerServices;
namespace Reolmarked.Repository.IRepo
{

    

    // Klassen repræsenterer en model, der kan notificere UI om ændringer i dens egenskaber
    public class YourClass : INotifyPropertyChanged
    {
        // Event som UI (fx WPF) kan "lytte" på for at blive opdateret, når en property ændres
        public event PropertyChangedEventHandler PropertyChanged;

        // Hjælpe-metode til at rejse PropertyChanged-eventen
        protected virtual void OnPropertyChanged(string propertyName)
        {
            // Tjekker om nogen abonnerer på eventen, og sender besked om hvilken property der ændrede sig
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Privat felt til at gemme værdien af RentalStatusId
        private int _rentalStatusId;

        // Egenskab for RentalStatusId
        public int RentalStatusId
        {
            get { return _rentalStatusId; }       // Returnerer den nuværende værdi
            set
            {
                _rentalStatusId = value;           // Sætter ny værdi
                OnPropertyChanged("RentalStatusId"); // Sender besked til UI om ændringen
            }
        }

        // Privat felt til at gemme værdien af RentalStatusName
        private string _rentalStatusName;

        // Egenskab for RentalStatusName
        public string RentalStatusName
        {
            get { return _rentalStatusName; }       // Returnerer den nuværende værdi
            set
            {
                _rentalStatusName = value;          // Sætter ny værdi
                OnPropertyChanged("RentalStatusName"); // Sender besked til UI om ændringen
            }
        }
    }

}
