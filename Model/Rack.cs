using System;
using System.Collections.Generic;

namespace Reolmarked.Model
{
    // Klassen repræsenterer et Rack (reol), som kan lejes eller reserveres
    public class Rack
    {
        // Unik identifikator for racket (PK i databasen)
        public int RackId { get; set; }

        // Navn eller beskrivelse af racket
        public string RackName { get; set; }

        // Angiver om racket er ledigt (true = ledigt, false = optaget)
        public bool IsAvailable { get; set; }

        // Angiver om racket er optaget (alternativ til IsAvailable)
        public bool IsOccupied { get; internal set; }

        // Navn på produkt placeret i racket (hvis relevant)
        public string ProductName { get; internal set; }

        // Liste af bøjler, der hører til racket
        public List<Hanger> Hangers { get; set; }

        // Hjælp til visning i fx ListView
        public override string ToString()
        {
            return RackId + ": " + RackName + " - " + (IsAvailable ? "Ledig" : "Optaget");
        }

        // Standard konstruktør
        public Rack()
        {
            Hangers = new List<Hanger>(); // sørger for at listen aldrig er null
        }

        // Konstruktør hvor vi kan oprette et Rack direkte med værdier
        public Rack(int rackId, string rackName, bool isAvailable)
        {
            RackId = rackId;
            RackName = rackName;
            IsAvailable = isAvailable;
            Hangers = new List<Hanger>();
        }
    }

   
}
