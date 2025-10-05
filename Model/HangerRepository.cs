using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Reolmarked.Model
{
    public class HangerRepository
    {
        // Forbindelsesstreng til SQL Server
        private string connectionString = "Server=Server01.;Database=ReolmarkedDB;Trusted_Connection=True;";

        // Metode til at hente alle hangers
        public List<Hanger> GetAllHangers()
        {
            List<Hanger> hangers = new List<Hanger>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // SQL forespørgsel til at hente hangers
                string sql = "SELECT HangerId, HangerName, RackId, IsAvailable FROM Hangers";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                // Læs alle rækker
                while (reader.Read())
                {
                    Hanger hanger = new Hanger();
                    hanger.HangerId = (int)reader["HangerId"];
                    hanger.HangerName = reader["HangerName"].ToString();
                    hanger.RackId = (int)reader["RackId"];
                    hanger.IsAvailable = (bool)reader["IsAvailable"];

                    hangers.Add(hanger);
                }
            }

            return hangers;
        }
    }
}
