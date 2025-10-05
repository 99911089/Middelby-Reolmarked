using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Reolmarked.Model
{
    public class CustomerRepository
    {
        private readonly string connectionString =
            "Server=Server01;Database=ReolmarkedDB;Trusted_Connection=True;";

        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT CustomerId, CustomerName, CustomerEmail, CustomerPhone FROM Customer";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Customer c = new Customer
                    {
                        CustomerId = (int)reader["CustomerId"],
                        CustomerName = reader["CustomerName"].ToString(),
                        CustomerEmail = reader["CustomerEmail"].ToString(),
                        CustomerPhone = reader["CustomerPhone"].ToString()
                    };

                    customers.Add(c);
                }
            }

            return customers;
        }

        public int AddCustomer(Customer c)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO Customer (CustomerName, CustomerEmail, CustomerPhone)
                               OUTPUT INSERTED.CustomerId
                               VALUES (@n, @e, @p)";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@n", c.CustomerName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@e", c.CustomerEmail ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p", (object?)c.CustomerPhone ?? DBNull.Value);

                int newId = (int)cmd.ExecuteScalar();
                return newId;
            }
        }

        public void DeleteCustomer(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Customer WHERE CustomerId = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", customerId);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE Customer " +
                             "SET CustomerName = @Name, CustomerEmail = @Email, CustomerPhone = @Phone " +
                             "WHERE CustomerId = @Id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", customer.CustomerName);
                cmd.Parameters.AddWithValue("@Email", customer.CustomerEmail);
                cmd.Parameters.AddWithValue("@Phone", customer.CustomerPhone);
                cmd.Parameters.AddWithValue("@Id", customer.CustomerId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
