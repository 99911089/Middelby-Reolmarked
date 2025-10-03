namespace Reolmarked.Model
{
    // Klassen repræsenterer en kunde/lejer (Tenant)
    public class Tenant
    {
        // Primærnøgle fra databasen
        public int TenantId { get; set; }

        // Kundens navn
        public string TenantName { get; set; }

        // Kundens email
        public string TenantEmail { get; set; }

        // Kundens telefonnummer
        public string TenantPhone { get; set; }

        // Bruges til visning i ListView mv.
        public override string ToString()
        {
            return $"{TenantId}: {TenantName} ({TenantEmail}, {TenantPhone})";
        }
    }
}
