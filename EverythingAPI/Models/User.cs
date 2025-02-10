using System.Text.RegularExpressions;

namespace EverythingAPI.Models
{
    public class User 
    {
        // Database creates ID
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public ICollection<Board> Boards { get; set; }

        public User (int userId, string userName, string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userName) || !IsNameValid(userName))
            {
                throw new ArgumentException("Name contains invalid characters or numbers");
            }

            if (string.IsNullOrWhiteSpace(userEmail) || !IsEmailValid(userEmail))
            {
                throw new ArgumentException("Email contains invalid characters");
            }

            UserId = userId;
            UserName = userName;
            UserEmail = userEmail;
            Boards = new List<Board>();
        }

        private bool IsNameValid(string name)
        {
            return Regex.IsMatch(name, @"^[a-zA-Z\s]+$");
        }

        private bool IsEmailValid(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
            return emailRegex.IsMatch(email);
        }

        public void addBoardToUser(Board board)
        {
            Boards.Add(board);
        }
    }
}
