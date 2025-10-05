using System;
using System.ComponentModel; // Giver adgang til INotifyPropertyChanged

namespace Reolmarked.Model
{
    // Klassen repræsenterer et produkt i Reolmarked-systemet.
    // INotifyPropertyChanged gør, at WPF automatisk opdaterer UI, når properties ændres.
    public class Product : INotifyPropertyChanged
    {
        // ===========================
        // Felter (private variabler)
        // ===========================
        private int productId;
        private string productName;
        private double price;
        private string barcode;
        private int ownerCustomerId;
        private bool isSold;
        private DateTime? soldDate;

        // ===========================
        // Egenskaber (public properties)
        // ===========================

        // Unikt ID for produktet
        public int ProductId
        {
            get { return productId; }
            set
            {
                productId = value;
                OnPropertyChanged("ProductId"); // Giv besked til UI om ændring
            }
        }

        // Produktets navn
        public string ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                OnPropertyChanged("ProductName"); // Giv besked til UI
            }
        }

        // Pris i kroner
        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        // Produktets stregkode
        public string Barcode
        {
            get { return barcode; }
            set
            {
                barcode = value;
                OnPropertyChanged("Barcode");
            }
        }

        // Kundens ID, som ejer produktet
        public int OwnerCustomerId
        {
            get { return ownerCustomerId; }
            set
            {
                ownerCustomerId = value;
                OnPropertyChanged("OwnerCustomerId");
            }
        }

        // Om produktet er solgt
        public bool IsSold
        {
            get { return isSold; }
            set
            {
                isSold = value;
                OnPropertyChanged("IsSold");
            }
        }

        // Dato for hvornår produktet blev solgt (kan være null)
        public DateTime? SoldDate
        {
            get { return soldDate; }
            set
            {
                soldDate = value;
                OnPropertyChanged("SoldDate");
            }
        }

        // Følgende properties beholdes som interne,
        // men uden notifikation, da de ikke bruges til data binding i UI.
        public string ProductName_ { get; internal set; }
        public object TenantId { get; internal set; }
        public string Name { get; internal set; }
        public object CustomerId { get; internal set; }

        // Tekst der vises fx i en liste (ListView eller ComboBox)
        public override string ToString()
        {
            return ProductName + " - " + Price + " kr.";
        }

        // ===========================
        // INotifyPropertyChanged
        // ===========================

        // Event der bruges til at informere UI om, at noget er ændret
        public event PropertyChangedEventHandler PropertyChanged;

        // Metode der kaldes, når en property ændres
        protected void OnPropertyChanged(string propertyName)
        {
            // Hvis nogen lytter (fx WPF’s data binding), så send besked
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
