using Microsoft.Data.SqlClient;
using Reolmarked.Model;
using Reolmarked.Repository.IRepo;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Reolmarked.Repository.DbRepo
{
    /// <summary>
    /// Repository klasse til at håndtere CRUD-operationer (Create, Read, Update, Delete)
    /// for tabellen "Rack" i databasen.
    /// </summary>
    public class DbRackRepository : IRackRepository
    {
        // Forbindelsesstreng til databasen (hentes fra App.config normalt)
        // Eksempel: "Server=Server01;Database=ReolmarkedDb;Trusted_Connection=True;Encrypt=False;"
        private readonly string _connectionString = "Server=Server01;Database=ReolmarkedDb;Trusted_Connection=True;Encrypt=False;";

        /// <summary>
        /// Henter alle racks (reoler) fra databasen.
        /// </summary>
        public List<Rack> GetAllRacks()
        {
            var racks = new List<Rack>();

            using (var conn = new SqlConnection(_connectionString))
            // OBS: Tabellens navn er "Rack", ikke "Racks"
            using (var cmd = new SqlCommand("SELECT RackId, RackName, IsAvailable FROM Racks", conn))
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var rack = new Rack
                        {
                            RackId = rdr["RackId"] != DBNull.Value ? Convert.ToInt32(rdr["RackId"]) : 0,
                            RackName = rdr["RackName"] != DBNull.Value ? rdr["RackName"].ToString() : string.Empty,
                            IsAvailable = rdr["IsAvailable"] != DBNull.Value && Convert.ToBoolean(rdr["IsAvailable"])
                        };

                        racks.Add(rack);
                    }
                }
            }

            return racks;
        }

        /// <summary>
        /// Tilføjer en ny rack til databasen.
        /// </summary>
        public void AddRack(Rack rack)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("INSERT INTO Rack (RackName, IsAvailable) VALUES (@Name, @Available)", conn))
            {
                cmd.Parameters.AddWithValue("@Name", rack.RackName);
                cmd.Parameters.AddWithValue("@Available", rack.IsAvailable);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Opdaterer en eksisterende rack i databasen.
        /// </summary>
        public void UpdateRack(Rack rack)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("UPDATE Rack SET RackName=@Name, IsAvailable=@Available WHERE RackId=@Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", rack.RackId);
                cmd.Parameters.AddWithValue("@Name", rack.RackName);
                cmd.Parameters.AddWithValue("@Available", rack.IsAvailable);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Sletter en rack fra databasen baseret på dens ID.
        /// </summary>
        public void DeleteRack(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM Rack WHERE RackId=@Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Finder en rack baseret på dens ID.
        /// </summary>
        public Rack GetRackById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT RackId, RackName, IsAvailable FROM Rack WHERE RackId=@Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        return new Rack
                        {
                            RackId = rdr.GetInt32(0),
                            RackName = rdr.GetString(1),
                            IsAvailable = rdr.GetBoolean(2)
                        };
                    }
                }
            }

            return null; // Hvis rack ikke findes
        }

        /// <summary>
        /// Henter alle ledige racks.
        /// </summary>
        public IEnumerable<Rack> GetAvailableRacks()
        {
            var racks = new List<Rack>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT RackId, RackName, IsAvailable FROM Rack WHERE IsAvailable=1", conn))
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        racks.Add(new Rack
                        {
                            RackId = rdr.GetInt32(0),
                            RackName = rdr.GetString(1),
                            IsAvailable = rdr.GetBoolean(2)
                        });
                    }
                }
            }

            return racks;
        }

        /// <summary>
        /// Henter alle optagede racks.
        /// </summary>
        public IEnumerable<Rack> GetOccupiedRacks()
        {
            var racks = new List<Rack>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT RackId, RackName, IsAvailable FROM Rack WHERE IsAvailable=0", conn))
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        racks.Add(new Rack
                        {
                            RackId = rdr.GetInt32(0),
                            RackName = rdr.GetString(1),
                            IsAvailable = rdr.GetBoolean(2)
                        });
                    }
                }
            }

            return racks;
        }
    }
}
