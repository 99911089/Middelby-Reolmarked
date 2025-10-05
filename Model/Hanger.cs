using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class Hanger
    {
        // Unik identifikator for bøjlen (PK i databasen)
        public int HangerId { get; set; }

        // Navn eller beskrivelse af bøjlen
        public string HangerName { get; set; }

        // ID for den rack bøjlen tilhører (FK i databasen)
        public int RackId { get; set; }

        // Angiver om bøjlen er ledig
        public bool IsAvailable { get; set; }

        // Hjælp til visning i fx ListView
        public override string ToString()
        {
            return HangerId + ": " + HangerName + " - " + (IsAvailable ? "Ledig" : "Optaget");
        }

        // Standard konstruktør
        public Hanger() { }

        // Konstruktør hvor vi kan oprette en hanger direkte med værdier
        public Hanger(int hangerId, string hangerName, int rackId, bool isAvailable)
        {
            HangerId = hangerId;
            HangerName = hangerName;
            RackId = rackId;
            IsAvailable = isAvailable;
        }
    }
}
