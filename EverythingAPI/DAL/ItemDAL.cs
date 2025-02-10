using System.Linq;
using Microsoft.Data.SqlClient;
using EverythingAPI.Models;
using System.Text.RegularExpressions;

namespace EverythingAPI.DAL
{
    public class ItemDAL
    {
        private readonly string connectionString;

        public ItemDAL(DatabaseConfig dbConfig)
        {
            connectionString = dbConfig.ConnectionString;
        }


        public async Task<List<Items>> RetriveAllItemsCorrespondingWithABoard(int boardID)
        {
            List<Items> items = new();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Items WHERE BoardID = @boardID", connection))
                {
                    command.Parameters.AddWithValue("@boardID", boardID);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Items Item = new Items(
                                itemId: (int)reader["ID"],
                                itemName: (string)reader["ItemName"],
                                itemDescription: (string)reader["ItemDescription"],
                                statusId: (int)reader["StatusID"],
                                boardId: (int)reader["BoardID"]                               
                                );
                            items.Add(Item);
                        }
                    }
                }
            }

            return items;
        }

        public async Task CreateItem(string itemName, string itemDescription, int statusId, int boardId)
        {
            var regexName = new Regex("^[a-zA-Z0-9 ]+$");
            if (!regexName.IsMatch(itemName))
            {
                throw new ArgumentException("Only: Letters, Numbers and Spaces");
            }

            var regexDescription = new Regex("^[a-zA-Z0-9.,;?!()&%$#@'\" ]+$");
            if (!regexDescription.IsMatch(itemDescription))
            {
                throw new ArgumentException("Invalid Characters.");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("INSERT INTO Items (ItemName, ItemDescription, StatusID, BoardID) VALUES (@itemName, @itemDescription, @statusID, @boardID)", connection))
                {
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.Parameters.AddWithValue("@itemDescription", itemDescription);
                    command.Parameters.AddWithValue("@statusID", statusId);
                    command.Parameters.AddWithValue("@boardID", boardId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteItem(int itemId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("DELETE FROM Items WHERE ID = @itemId", connection))
                {
                    command.Parameters.AddWithValue("@itemId", itemId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task ChangeItemStatus(int itemId, int itemStatusId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("UPDATE Items SET StatusId = @itemStatusId WHERE ID = @itemId", connection))
                {
                    command.Parameters.AddWithValue("@itemId", itemId);
                    command.Parameters.AddWithValue("@itemStatusId", itemStatusId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
