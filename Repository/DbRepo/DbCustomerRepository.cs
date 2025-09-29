using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Reolmarked.Model;
using Reolmarked.Repository.IRepo;

namespace Reolmarked.Repository.DbRepo
{
    public class DbCustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString; // Gemmer database connection string

        public string connectionString { get; private set; }

        // Konstruktør med connection string
        public DbCustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Opret ny kunde
        public void CreateCustomer(Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Customers (CustomerName, CustomerEmail, CustomerPhone) VALUES (@name, @email, @phone)";
                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@name", customer.CustomerName);
                command.Parameters.AddWithValue("@email", customer.CustomerEmail);
                command.Parameters.AddWithValue("@phone", customer.CustomerPhone);

                command.ExecuteNonQuery();
            }
        }

        // Hent alle kunder
        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT CustomerId, CustomerName, CustomerEmail, CustomerPhone FROM Customer";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            CustomerId = reader.GetInt32(0),
                            CustomerName = reader.GetString(1),
                            CustomerEmail = reader.GetString(2),
                            CustomerPhone = reader.GetString(3)
                        });
                    }
                }
            }

            return customers;
        }



        // Hent kunde efter ID
        public Customer GetById(int customerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT CustomerId, CustomerName, CustomerEmail, CustomerPhone FROM Customers WHERE CustomerId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", customerId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Customer
                        {
                            CustomerId = reader.GetInt32(0),
                            CustomerName = reader.GetString(1),
                            CustomerEmail = reader.GetString(2),
                            CustomerPhone = reader.GetString(3)
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        // Opdater kunde
        public void UpdateCustomer(Customer customer)
        {
            // Husk at ændre "Customers" til dit tabelnavn i databasen
            string sql = @"
        UPDATE Customers
        SET Name = @Name,
            Email = @Email,
            PhoneNumber = @Phone
        WHERE CustomerId = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Bind parametre
                    command.Parameters.AddWithValue("@Name", customer.CustomerName);
                    command.Parameters.AddWithValue("@Email", customer.CustomerEmail);
                    command.Parameters.AddWithValue("@Phone", customer.CustomerPhone);
                    command.Parameters.AddWithValue("@Id", customer.CustomerId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        // Slet kunde
        public void DeleteCustomer(int customerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM Customers WHERE CustomerId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", customerId);

                command.ExecuteNonQuery();
            }
        }
    }
}
