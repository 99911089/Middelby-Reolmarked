using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Reolmarked.Model
{
    public class RackRepository
    {
        private readonly string connectionString =
            "Server=Server01;Database=ReolmarkedDB;Trusted_Connection=True;";

        // Hent alle reoler inkl. deres bøjler
        public List<Rack> GetAllRacks()
        {
            List<Rack> racks = new List<Rack>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT RackId, RackName, IsAvailable FROM Racks";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Rack rack = new Rack
                    {
                        RackId = (int)reader["RackId"],
                        RackName = reader["RackName"].ToString(),
                        IsAvailable = (bool)reader["IsAvailable"],
                        Hangers = new List<Hanger>() // vi udfylder den bagefter
                    };

                    racks.Add(rack);
                }
            }

            // Tilføj bøjler til hver reol
            foreach (Rack r in racks)
            {
                r.Hangers = GetHangersForRack(r.RackId);
            }

            return racks;
        }

        // Hent alle bøjler for en given reol
        private List<Hanger> GetHangersForRack(int rackId)
        {
            List<Hanger> hangers = new List<Hanger>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT HangerId, HangerName, IsAvailable FROM Hangers WHERE RackId = @rackId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rackId", rackId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Hanger h = new Hanger
                    {
                        HangerId = (int)reader["HangerId"],
                        HangerName = reader["HangerName"].ToString(),
                        IsAvailable = (bool)reader["IsAvailable"]
                    };

                    hangers.Add(h);
                }
            }

            return hangers;
        }

        // Tilføj en ny reol
        public int AddRack(Rack rack)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "INSERT INTO Racks (RackName, IsAvailable) OUTPUT INSERTED.RackId VALUES (@name, @available)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", rack.RackName);
                cmd.Parameters.AddWithValue("@available", rack.IsAvailable);

                int newId = (int)cmd.ExecuteScalar();
                return newId;
            }
        }

        // Opdater en eksisterende reol
        public void UpdateRack(Rack rack)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "UPDATE Racks SET RackName=@name, IsAvailable=@available WHERE RackId=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", rack.RackId);
                cmd.Parameters.AddWithValue("@name", rack.RackName);
                cmd.Parameters.AddWithValue("@available", rack.IsAvailable);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
