using Microsoft.EntityFrameworkCore;
using Reolmarked.Model;

namespace Reolmarked.Data
{
    // DbContext repræsenterer forbindelsen til databasen
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        // Angiv hvilken database der bruges (her SQLite)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Database-fil ligger lokalt i projektmappen
                optionsBuilder.UseSqlite("Data Source=reolmarked.db");
            }
        }
    }
}

