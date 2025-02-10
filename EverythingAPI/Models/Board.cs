namespace EverythingAPI.Models
{
    public class Board 
    {
        // Database creates ID
        public int? BoardId { get; set; }
        public string BoardName { get; set; }
        public int UserId { get; set; }
        public ICollection<Items> Items { get; set; }

        public Board(int boardId, string boardName, int userId)
        {
            BoardId = boardId;
            BoardName = boardName;
            UserId = userId;
            Items = new List<Items>();
        }

        public void AddItemToBoard(Items item)
        {
            Items.Add(item);
        }
    }
}
