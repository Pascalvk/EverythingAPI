using System.Linq;
using Microsoft.Data.SqlClient;
using EverythingAPI.Models;
using System.Text.RegularExpressions;

namespace EverythingAPI.DAL
{
    public class BoardDAL
    {
        private readonly string connectionString;

        public BoardDAL(DatabaseConfig dbConfig)
        {
            connectionString = dbConfig.ConnectionString;
        }


        public async Task<List<Board>> RetriveAllBoardsCorrespondingWithUser(int userID)
        {
            List<Board> Boards = new();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Board WHERE UserID = @userID", connection))
                {
                    command.Parameters.AddWithValue("@userID", userID);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Board Board = new Board(
                                boardId: (int)reader["ID"],
                                boardName: (string)reader["BoardName"],
                                userId: (int)reader["UserID"]
                                );
                            Boards.Add(Board);
                        }
                    }
                }
            }
            return Boards;
        }

        public async Task CreateBoard(string boardName, int userId)
        {
            var regex = new Regex("^[a-zA-Z0-9 ]+$");
            if (!regex.IsMatch(boardName))
            {
                throw new ArgumentException("Only: Letters, Numbers and Spaces");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("INSERT INTO Board (BoardName, UserID) VALUES (@boardName, @userID)", connection))
                {
                    command.Parameters.AddWithValue("@boardName", boardName);
                    command.Parameters.AddWithValue("@userID", userId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteBoard(int boardId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("DELETE FROM Board WHERE ID = @boardId", connection))
                {
                    command.Parameters.AddWithValue("@boardId", boardId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
