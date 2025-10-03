using Microsoft.Data.SqlClient;
using Reolmarked.Model;
using System.Collections.Generic;

namespace Reolmarked.Repository.DbRepo
{
    // Repository til at hente og gemme kunder (Tenants) i SQL-databasen
    public class DbTenantRepository
    {
        private readonly string _connectionString;

        // Konstruktør hvor vi giver connectionstring med fra App.config
        public DbTenantRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Hent ALLE kunder fra databasen
        public List<Tenant> GetAllTenants()
        {
            var tenants = new List<Tenant>();

            // Åbn forbindelse og hent alle rækker fra tabellen Tenants
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT TenantId, TenantName, TenantEmail, TenantPhone FROM Tenants", conn))
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    // Læs alle rækker og map til Tenant-objekter
                    while (rdr.Read())
                    {
                        tenants.Add(new Tenant
                        {
                            TenantId = rdr.GetInt32(0),
                            TenantName = rdr.GetString(1),
                            TenantEmail = rdr.GetString(2),
                            TenantPhone = rdr.GetString(3)
                        });
                    }
                }
            }

            return tenants;
        }

        // Tilføj en ny kunde
        public void AddTenant(Tenant tenant)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "INSERT INTO Tenants (TenantName, TenantEmail, TenantPhone) VALUES (@Name, @Email, @Phone)", conn))
            {
                cmd.Parameters.AddWithValue("@Name", tenant.TenantName);
                cmd.Parameters.AddWithValue("@Email", tenant.TenantEmail);
                cmd.Parameters.AddWithValue("@Phone", tenant.TenantPhone);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Opdater en eksisterende kunde
        public void UpdateTenant(Tenant tenant)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "UPDATE Tenants SET TenantName=@Name, TenantEmail=@Email, TenantPhone=@Phone WHERE TenantId=@Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", tenant.TenantId);
                cmd.Parameters.AddWithValue("@Name", tenant.TenantName);
                cmd.Parameters.AddWithValue("@Email", tenant.TenantEmail);
                cmd.Parameters.AddWithValue("@Phone", tenant.TenantPhone);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Slet en kunde
        public void DeleteTenant(int tenantId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM Tenants WHERE TenantId=@Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", tenantId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

