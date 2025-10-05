using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Reolmarked.Model
{
    public class ProductRepository
    {
        // Forbindelsesstreng til din SQL Server-database
        private readonly string connectionString = "Data Source=.;Initial Catalog=ReolmarkedDB;Integrated Security=True;";

        // ===================== HENT ALLE PRODUKTER =====================
        internal IEnumerable<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT ProductId, ProductName, Price, Barcode, CustomerId FROM Product";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product p = new Product();
                        p.ProductId = reader.GetInt32(0);
                        p.ProductName = reader.GetString(1);
                        p.Price = Convert.ToDouble(reader.GetDecimal(2));
                        p.Barcode = reader.GetString(3);
                        p.CustomerId = reader.GetInt32(4);

                        products.Add(p);
                    }
                }
            }

            return products;
        }

        // ===================== HENT PRODUKT VIA STREGKODE =====================
        internal Product GetProductByBarcode(string barcode)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT ProductId, ProductName, Price, Barcode, CustomerId FROM Product WHERE Barcode = @Barcode";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Barcode", barcode);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Product p = new Product();
                            p.ProductId = reader.GetInt32(0);
                            p.ProductName = reader.GetString(1);
                            p.Price = Convert.ToDouble(reader.GetDecimal(2));
                            p.Barcode = reader.GetString(3);
                            p.CustomerId = reader.GetInt32(4);
                            return p;
                        }
                    }
                }
            }

            return null;
        }

        // ===================== TILFØJ PRODUKT =====================
        internal int AddProduct(Product p)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO Product (ProductName, Price, Barcode, CustomerId) " +
                               "OUTPUT INSERTED.ProductId VALUES (@Name, @Price, @Barcode, @CustomerId)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", p.ProductName);
                    cmd.Parameters.AddWithValue("@Price", p.Price);
                    cmd.Parameters.AddWithValue("@Barcode", p.Barcode);
                    cmd.Parameters.AddWithValue("@CustomerId", p.CustomerId);

                    // Returnér ID på det indsatte produkt
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        // ===================== OPDATER PRODUKT =====================
        internal void UpdateProduct(Product p)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "UPDATE Product SET ProductName = @Name, Price = @Price, Barcode = @Barcode, CustomerId = @CustomerId " +
                               "WHERE ProductId = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", p.ProductName);
                    cmd.Parameters.AddWithValue("@Price", p.Price);
                    cmd.Parameters.AddWithValue("@Barcode", p.Barcode);
                    cmd.Parameters.AddWithValue("@CustomerId", p.CustomerId);
                    cmd.Parameters.AddWithValue("@Id", p.ProductId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ===================== SLET PRODUKT =====================
        internal void DeleteProduct(int productId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM Product WHERE ProductId = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
