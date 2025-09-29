using Reolmarked.Model.Reolmarked.Model;
using System.Collections.Generic;

namespace Reolmarked.Model
{
    // Håndterer alle reoler og salg
    public class RackMarket
    {
        public List<Rack> Racks { get; set; } = new List<Rack>();

        // Hent alle ledige reoler
        public List<Rack> GetAvailableRacks()
        {
            List<Rack> available = new List<Rack>();
            foreach (var rack in Racks)
            {
                if (!rack.IsOccupied) // Tjek om reolen ikke er optaget
                {
                    available.Add(rack);
                }
            }
            return available;
        }

        // Hent alle optagede reoler
        public List<Rack> GetOccupiedRacks()
        {
            List<Rack> occupied = new List<Rack>();
            foreach (var rack in Racks)
            {
                if (rack.IsOccupied) // Tjek om reolen er optaget
                {
                    occupied.Add(rack);
                }
            }
            return occupied;
        }

        // Hent alle reoler (for oversigt)
        public List<Rack> GetAllRacks()
        {
            List<Rack> allRacks = new List<Rack>();
            foreach (var rack in Racks)
            {
                allRacks.Add(rack); // Tilføj hver reol til listen
            }
            return allRacks;
        }

        // Sælg produkt på en reol
        public double SellProduct(int rackId, double salePrice)
        {
            foreach (var rack in Racks)
            {
                if (rack.RackId == rackId && rack.IsOccupied) // Find korrekt reol og tjek om optaget
                {
                    Sale sale = new Sale { SalePrice = salePrice }; // Opret salg
                    double commission = sale.CalculateCommission(); // Beregn kommission

                    // Fjern produkt fra reolen
                    rack.IsOccupied = false;
                    rack.ProductName = null;

                    return commission; // Returner kommission
                }
            }
            return 0; // Returner 0, hvis intet salg
        }
    }
}
