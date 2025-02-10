using System.Linq;
using Microsoft.Data.SqlClient;
using EverythingAPI.Models;
using System.Text.RegularExpressions;


namespace EverythingAPI.DAL
{
    public class UserDAL
    {
        private readonly string connectionString;

        public UserDAL(DatabaseConfig dbConfig)
        {
            connectionString = dbConfig.ConnectionString;
        }


        public async Task<List<User>> RetrieveAllUsers()
        {
            List<User> users = new();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"
            SELECT 
                u.ID AS UserID, u.UserName, u.Email, 
                b.ID AS BoardID, b.BoardName
            FROM Users u
            LEFT JOIN Board b ON u.ID = b.UserID
            ORDER BY u.ID, b.ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        Dictionary<int, User> userDict = new();

                        while (await reader.ReadAsync())
                        {
                            int userId = (int)reader["UserID"];

                            if (!userDict.TryGetValue(userId, out User user))
                            {
                                user = new User
                                (
                                    userId: userId,
                                    userName: (string)reader["UserName"],
                                    userEmail: (string)reader["Email"]
                                );
                                userDict[userId] = user;
                            }

                            if (!(reader["BoardID"] is DBNull))
                            {
                                Board board = new Board
                                (
                                    boardId: (int)reader["BoardID"],
                                    boardName: (string)reader["BoardName"],
                                    userId: userId
                                );
                                user.Boards.Add(board);
                            }
                        }

                        users = userDict.Values.ToList();
                    }
                }
            }
            return users;
        }

        public async Task<List<User>> RetrieveSpecificUser(string email)
        {
            List<User> Users = new();
            using (SqlConnection connection = new SqlConnection (connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            User User = new User(
                                userId: (int)reader["ID"],
                                userName: (string)reader["UserName"],
                                userEmail: (string)reader["Email"]
                                );
                            Users.Add(User);
                        }
                    }
                }
            }
            return Users;
        }


        public async Task CreateNewUser(string userName, string userEmail)
        {
            var regexName = new Regex("^[a-zA-Z0-9 ]+$");
            if (!regexName.IsMatch(userName))
            {
                throw new ArgumentException("Only: Letters, Numbers and Spaces");
            }

            var regexEmail = new Regex("^[a-zA-Z0-9]+([._%+-]?[a-zA-Z0-9]+)*@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,}$");
            if (!regexEmail.IsMatch(userEmail))
            {
                throw new ArgumentException("Not a Valid Email");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("INSERT INTO Users (UserName, Email) VALUES (@userName, @userEmail)", connection))
                {
                    command.Parameters.AddWithValue("@userName", userName);
                    command.Parameters.AddWithValue("@userEmail", userEmail);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("DELETE FROM Users WHERE ID = @userId", connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<User>> RetrieveSpecificUserAllData(string email)
        {
            List<User> Users = new();
            Dictionary<int, User> userDictionary = new();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(
                    "SELECT Users.ID AS UserID, Users.UserName, Users.Email, " +
                    "Board.ID AS BoardID, Board.BoardName, " +
                    "Items.ID AS ItemID, Items.ItemName, Items.ItemDescription, Items.StatusID, " +
                    "StatusItem.ID AS StatusItemID, StatusItem.StatusName " +
                    "FROM Users " +
                    "LEFT JOIN Board ON Users.Id = Board.UserID " +
                    "LEFT JOIN Items ON Board.Id = Items.BoardID " +
                    "LEFT JOIN StatusItem ON Items.StatusID = StatusItem.ID " +
                    "WHERE Users.Email = @Email " +
                    "ORDER BY Board.ID, Items.ID;", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int userId = (int)reader["UserID"];

                            if (!userDictionary.ContainsKey(userId))
                            {
                                var user = new User(
                                    userId: userId,
                                    userName: (string)reader["UserName"],
                                    userEmail: (string)reader["Email"]
                                );
                                userDictionary[userId] = user;
                                Users.Add(user);
                            }

                            var userFullData = userDictionary[userId];
                            int boardId = reader["BoardID"] == DBNull.Value ? 0 : (int)reader["BoardID"];

                            var board = userFullData.Boards.FirstOrDefault(b => b.BoardId == boardId);
                            if (board == null && boardId != 0)
                            {
                                board = new Board(
                                    boardId: boardId,
                                    boardName: (string)reader["BoardName"],
                                    userId: userId
                                );
                                userFullData.Boards.Add(board);
                            }

                            int itemId = reader["ItemID"] == DBNull.Value ? 0 : (int)reader["ItemID"];
                            if (board != null && itemId != 0)
                            {
                                var item = new Items(
                                    itemId: itemId,
                                    itemName: (string)reader["ItemName"],
                                    itemDescription: (string)reader["ItemDescription"],
                                    statusId: (int)reader["StatusID"],
                                    boardId: boardId
                                );

                                int statusItemId = reader["StatusItemID"] == DBNull.Value ? 0 : (int)reader["StatusItemID"];
                                if (statusItemId != 0)
                                {
                                    item.StatusItems.Add(new StatusItem(
                                        statusItemId: statusItemId,
                                        statusName: (string)reader["StatusName"]
                                    ));
                                }

                                board.Items.Add(item);
                            }
                        }
                    }
                }
            }

            return Users;
        }

    }
}
