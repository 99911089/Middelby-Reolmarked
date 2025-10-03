namespace Reolmarked
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
        public bool IsOccupied { get; internal set; }
        public object ProductName { get; internal set; }

        // Hjælp til visning i fx ListView
        public override string ToString()
        {
            return $"{RackId}: {RackName} - {(IsAvailable ? "Ledig" : "Optaget")}";
        }

        // Standard konstruktør
        public Rack()
        {
        }

        // Konstruktør hvor vi kan oprette et Rack direkte med værdier
        public Rack(int rackId, string rackName, bool isAvailable)
        {
            RackId = rackId;
            RackName = rackName;
            IsAvailable = isAvailable;
        }
    }
}
