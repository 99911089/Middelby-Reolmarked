using System;

namespace Reolmarked.Model
{
    public class Sale
    {
        // Unikt ID for hvert salg (primærnøgle i databasen)
        public int SaleId { get; set; }

        // Navn på produktet, som blev solgt
        public string ProductName { get; set; }

        // Pris på produktet
        public int Price { get; set; }

        // Stregkode for produktet
        public string Barcode { get; set; }

        // Dato for hvornår produktet blev solgt (kan være null)
        public DateTime? SoldDate { get; set; }

        // Kunde-ID, der ejer/har købt produktet (kan være null)
        public int? CustomerId { get; set; }
        public double SalePrice { get; internal set; }
        public int ProductId { get; internal set; }

        // Metode til at vise et læsevenligt format i fx ListView
        public override string ToString()
        {
            // Hvis der er en dato, brug den – ellers skriv "Ingen dato"
            string dateText = SoldDate.HasValue ? SoldDate.Value.ToShortDateString() : "Ingen dato";

            // Returnér tekst med dato, navn og pris
            return $"{dateText} - {ProductName} ({Price} kr.)";
        }

        internal double CalculateCommission()
        {
            throw new NotImplementedException();
        }
    }
}
