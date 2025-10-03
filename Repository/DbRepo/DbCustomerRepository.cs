using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient; // Husk denne nuget: Microsoft.Data.SqlClient
using Reolmarked.Model;
using Reolmarked.Repository.IRepo;

namespace Reolmarked.Repository.DbRepo
{
    /// <summary>
    /// Database-repository for Customers (CRUD).
    /// </summary>
    public class DbCustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public DbCustomerRepository()
        {
            // Hent connection string fra App.config
            var cs = ConfigurationManager.ConnectionStrings["ReolmarkedDb"];
            if (cs == null)
                throw new InvalidOperationException("Connection string 'ReolmarkedDb' mangler i App.config!");
            _connectionString = cs.ConnectionString;
        }

        /// <summary>
        /// Henter alle kunder fra databasen
        /// </summary>
        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "SELECT CustomerId, CustomerName, CustomerEmail, CustomerPhone FROM Customers", conn))
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        customers.Add(new Customer
                        {
                            CustomerId = rdr.GetInt32(0),
                            CustomerName = rdr.GetString(1),
                            CustomerEmail = rdr.IsDBNull(2) ? null : rdr.GetString(2),
                            CustomerPhone = rdr.IsDBNull(3) ? null : rdr.GetString(3)
                        });
                    }
                }
            }
            return customers;
        }

        /// <summary>
        /// Tilføjer en ny kunde
        /// </summary>
        public void AddCustomer(Customer newCustomer)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "INSERT INTO Customers (CustomerName, CustomerEmail, CustomerPhone) VALUES (@CustomerName, @CustomerEmail, @CustomerPhone)", conn))
            {
                cmd.Parameters.AddWithValue("@CustomerName", newCustomer.CustomerName);
                cmd.Parameters.AddWithValue("@CustomerEmail", (object?)newCustomer.CustomerEmail ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CustomerPhone", (object?)newCustomer.CustomerPhone ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Opdaterer en eksisterende kunde
        /// </summary>
        public void UpdateCustomer(Customer customer)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "UPDATE Customers SET CustomerName=@CustomerName, CustomerEmail=@CustomerEmail, CustomerPhone=@CustomerPhone WHERE CustomerId=@CustomerId", conn))
            {
                cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                cmd.Parameters.AddWithValue("@CustomerEmail", (object?)customer.CustomerEmail ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CustomerPhone", (object?)customer.CustomerPhone ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Sletter en kunde ud fra ID
        /// </summary>
        public void DeleteCustomer(int customerId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM Customers WHERE CustomerId=@CustomerId", conn))
            {
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void CreateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
