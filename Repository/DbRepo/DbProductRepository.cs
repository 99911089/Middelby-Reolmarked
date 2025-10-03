using Microsoft.Data.SqlClient;
using Reolmarked.Model;
using Reolmarked.Repository.IRepo;
using System;
using System.Collections.Generic;

namespace Reolmarked.Repository.DbRepo
{
    public class DbProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public DbProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Hent alle produkter
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT ProductId, ProductName, Price, Barcode, TenantId FROM Products", conn))
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        products.Add(new Product
                        {
                            ProductId = rdr.GetInt32(0),
                            ProductName = rdr.GetString(1),
                            Price = (double)rdr.GetDecimal(2),
                            Barcode = rdr.GetString(3),
                            TenantId = rdr.IsDBNull(4) ? null : rdr.GetInt32(4)
                        });
                    }
                }
            }
            return products;
        }

        // Tilføj produkt
        public void AddProduct(Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "INSERT INTO Products (ProductName, Price, Barcode, TenantId) VALUES (@Name, @Price, @Barcode, @TenantId)", conn))
            {
                cmd.Parameters.AddWithValue("@Name", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Barcode", product.Barcode);
                cmd.Parameters.AddWithValue("@TenantId", (object?)product.TenantId ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Opdater produkt
        public void UpdateProduct(Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "UPDATE Products SET ProductName=@Name, Price=@Price, Barcode=@Barcode, TenantId=@TenantId WHERE ProductId=@Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", product.ProductId);
                cmd.Parameters.AddWithValue("@Name", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Barcode", product.Barcode);
                cmd.Parameters.AddWithValue("@TenantId", (object?)product.TenantId ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Slet produkt
        public void DeleteProduct(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM Products WHERE ProductId=@Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

