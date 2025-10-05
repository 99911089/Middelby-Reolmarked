using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Reolmarked.Model
{
    public class SaleRepository
    {
        // ==========================================================
        // Forbindelsesstreng til SQL Server
        // ==========================================================
        private readonly string connectionString =
            "Server=Server01;Database=ReolmarkedDB;Trusted_Connection=True;";

        // ==========================================================
        // HENT ALLE SALG FRA DATABASEN
        // ==========================================================
        public List<Sale> GetAllSales()
        {
            List<Sale> sales = new List<Sale>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Hent alle rækker fra Sales-tabellen
                string query = "SELECT SaleId, ProductName, Price, Barcode, SoldDate, CustomerId FROM Sales";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Sale s = new Sale();

                    // SaleId (int)
                    s.SaleId = reader.GetInt32(0);

                    // Produktnavn (string)
                    s.ProductName = reader.GetString(1);

                    // Pris (decimal i DB, så vi konverterer korrekt)
                    if (reader["Price"] is decimal dec)
                        s.Price = Convert.ToInt32(dec);
                    else
                        s.Price = reader.GetInt32(2);

                    // Stregkode (string)
                    s.Barcode = reader.GetString(3);

                    // Solgt dato (kan være null)
                    if (reader.IsDBNull(4))
                        s.SoldDate = null;
                    else
                        s.SoldDate = reader.GetDateTime(4);

                    // KundeID (kan være null)
                    if (reader.IsDBNull(5))
                        s.CustomerId = null;
                    else
                        s.CustomerId = reader.GetInt32(5);

                    // Tilføj til liste
                    sales.Add(s);
                }
            }

            return sales;
        }

        // ==========================================================
        // INDSÆT ET NYT SALG I DATABASEN
        // ==========================================================
        public int AddSale(Sale sale)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Først: kontroller at CustomerId findes i CUSTOMER-tabellen
                EnsureValidCustomer(conn, sale.CustomerId);

                // SQL-kommando: indsæt og returnér det nye SaleId
                string sql = @"INSERT INTO Sale (ProductName, Price, Barcode, SoldDate, CustomerId)
                               OUTPUT INSERTED.SaleId
                               VALUES (@ProductName, @Price, @Barcode, @SoldDate, @CustomerId)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                // Produktnavn
                cmd.Parameters.AddWithValue("@ProductName", sale.ProductName ?? (object)DBNull.Value);

                // Pris
                cmd.Parameters.AddWithValue("@Price", sale.Price);

                // Stregkode
                cmd.Parameters.AddWithValue("@Barcode", sale.Barcode ?? (object)DBNull.Value);

                // Dato (kan være null)
                if (sale.SoldDate.HasValue)
                    cmd.Parameters.AddWithValue("@SoldDate", sale.SoldDate.Value);
                else
                    cmd.Parameters.AddWithValue("@SoldDate", DBNull.Value);

                // KundeID (kan være null)
                if (sale.CustomerId.HasValue)
                    cmd.Parameters.AddWithValue("@CustomerId", sale.CustomerId.Value);
                else
                    cmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);

                // Udfør og returnér nyt ID
                return (int)cmd.ExecuteScalar();
            }
        }

        // ==========================================================
        // OPDATER ET EKSISTERENDE SALG I DATABASEN
        // ==========================================================
        public void UpdateSale(Sale sale)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Sikrer at CustomerId findes i CUSTOMER-tabellen,
                // ellers sættes den til NULL for at undgå foreign key fejl
                EnsureValidCustomer(conn, sale.CustomerId);

                string sql = @"UPDATE Sales
                               SET ProductName = @ProductName,
                                   Price = @Price,
                                   Barcode = @Barcode,
                                   SoldDate = @SoldDate,
                                   CustomerId = @CustomerId
                               WHERE SaleId = @SaleId";

                SqlCommand cmd = new SqlCommand(sql, conn);

                // Produktnavn
                cmd.Parameters.AddWithValue("@ProductName", sale.ProductName ?? (object)DBNull.Value);

                // Pris
                cmd.Parameters.AddWithValue("@Price", sale.Price);

                // Stregkode
                cmd.Parameters.AddWithValue("@Barcode", sale.Barcode ?? (object)DBNull.Value);

                // Solgt dato (kan være null)
                if (sale.SoldDate.HasValue)
                    cmd.Parameters.AddWithValue("@SoldDate", sale.SoldDate.Value);
                else
                    cmd.Parameters.AddWithValue("@SoldDate", DBNull.Value);

                // KundeID (kan være null)
                if (sale.CustomerId.HasValue)
                    cmd.Parameters.AddWithValue("@CustomerId", sale.CustomerId.Value);
                else
                    cmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);

                // SaleId bruges til at finde den rigtige række
                cmd.Parameters.AddWithValue("@SaleId", sale.SaleId);

                cmd.ExecuteNonQuery();
            }
        }

        // ==========================================================
        // SLET ET SALG FRA DATABASEN
        // ==========================================================
        public void DeleteSale(int saleId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM Sales WHERE SaleId = @SaleId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SaleId", saleId);
                cmd.ExecuteNonQuery();
            }
        }

        // ==========================================================
        // HJÆLPEMETODE: TJEK OM CUSTOMERID FINDES I DB
        // ==========================================================
        private void EnsureValidCustomer(SqlConnection conn, int? customerId)
        {
            // Hvis ingen kunde valgt, sæt til NULL
            if (!customerId.HasValue)
                return;

            // SQL: tjek om kunden findes
            string sql = "SELECT COUNT(*) FROM CUSTOMER WHERE CustomerId = @CustomerId";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CustomerId", customerId.Value);

            int count = (int)cmd.ExecuteScalar();

            // Hvis kunden ikke findes, så fjern reference (ellers foreign key fejl)
            if (count == 0)
            {
                customerId = null;
            }
        }
    }
}
