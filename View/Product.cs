namespace Reolmarked.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Barcode { get; set; }
        public string ProductName_ { get; internal set; }

        public int OwnerCustomerId { get; set; } // Reference til kunden
        public bool IsSold { get; set; }
        public DateTime? SoldDate { get; set; }
        public object TenantId { get; internal set; }
        public string Name { get; internal set; }

        public override string ToString()
        {
            return $"{ProductName} - {Price} kr.";
        }

           

        
    }
}

