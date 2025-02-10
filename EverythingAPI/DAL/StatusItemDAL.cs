using System.Linq;
using Microsoft.Data.SqlClient;
using EverythingAPI.Models;

namespace EverythingAPI.DAL
{
    public class StatusItemDAL
    {
        private readonly string connectionString;

        public StatusItemDAL(DatabaseConfig dbConfig)
        {
            connectionString = dbConfig.ConnectionString;
        }


        public async Task<List<StatusItem>> RetrieveAllStatusItems()
        {
            List<StatusItem> StatusItems = new();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM StatusItem", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            StatusItem StatusItem = new StatusItem(
                                statusItemId: (int)reader["ID"],
                                statusName: (string)reader["StatusName"]
                                );
                            StatusItems.Add(StatusItem);
                        }
                    }
                }
            }
            return StatusItems;
        }

        public async Task<List<StatusItem>> RetrieveSpecificStatusItems(int statusID)
        {
            List<StatusItem> StatusItems = new();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM StatusItem WHERE ID = @statusID", connection))
                {
                    command.Parameters.AddWithValue("@statusID", statusID);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            StatusItem StatusItem = new StatusItem(
                                statusItemId: (int)reader["ID"],
                                statusName: (string)reader["StatusName"]
                                );
                            StatusItems.Add(StatusItem);
                        }
                    }
                }
            }
            return StatusItems;
        }

    }
}
