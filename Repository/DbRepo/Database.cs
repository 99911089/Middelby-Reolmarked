using Microsoft.Data.SqlClient;  // Indeholder SqlConnection, SqlCommand osv. til SQL Server
using System;

namespace Reolmarked.Repository.DbRepo
{
    // Klasse der sikrer at databasen og tabellen CUSTOMER eksisterer.
    // Opretter automatisk DB og tabel hvis de mangler.
    public class Database
    {
        // Egenskab til at vise seneste statusbesked (fx til UI eller log)
        public string LatestStatus { get; private set; }

        // Connection string til selve Reolmarked-databasen
        private readonly string _cs =
            @"Server=SERVER01;Database=ReolmarkedDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // Connection string til "master"-databasen (bruges til at oprette ny database)
        private readonly string _serverConnectionString =
            @"Server=SERVER01;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

        // Konstruktør: kaldes når klassen oprettes
        public Database()
        {
            LatestStatus = "Database-klassen oprettet.";

            // Sikrer at database og tabel eksisterer
            EnsureDatabaseExists();
            EnsureTableExists();
        }

        // Sikrer at databasen "ReolmarkedDB" findes, ellers oprettes den
        private void EnsureDatabaseExists()
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(_serverConnectionString); // Forbindelse til master-databasen
                conn.Open();

                string sql = @"
                    IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ReolmarkedDB')
                    BEGIN
                        CREATE DATABASE ReolmarkedDB;
                    END";

                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery(); // Kører SQL-scriptet
                LatestStatus = "Database tjekket/oprettet.";
            }
            catch (Exception ex)
            {
                LatestStatus = "Fejl ved databasekontrol: " + ex.Message;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();
            }
        }

        // Sikrer at tabellen CUSTOMER findes, ellers oprettes den
        private void EnsureTableExists()
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(_cs); // Forbindelse til ReolmarkedDB
                conn.Open();

                string sql = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CUSTOMER' AND xtype='U')
                    BEGIN
                        CREATE TABLE CUSTOMER (
                            CustomerId INT IDENTITY(1,1) PRIMARY KEY,
                            CustomerName NVARCHAR(100) NOT NULL,
                            CustomerEmail NVARCHAR(100) NOT NULL,
                            CustomerPhone NVARCHAR(50) NULL
                        );
                    END";

                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery(); // Kører SQL-scriptet
                LatestStatus = "Tabel tjekket/oprettet.";
            }
            catch (Exception ex)
            {
                LatestStatus = "Fejl ved tabelkontrol: " + ex.Message;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();
            }
        }

        // Returnerer connection string til ReolmarkedDB (bruges i repositories)
        public string GetConnectionString()
        {
            return _cs;
        }
    }
}
