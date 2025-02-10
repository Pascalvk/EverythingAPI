namespace EverythingAPI.Models
{
    public class Items
    {
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int StatusId { get; set; }
        public int BoardId { get; set; }

        public ICollection<StatusItem> StatusItems { get; set; }

        public Items(int itemId, string itemName, string itemDescription, int statusId, int boardId)
        {
            ItemId = itemId;
            ItemName = itemName;
            ItemDescription = itemDescription;
            StatusId = statusId;
            BoardId = boardId;
            StatusItems = new List<StatusItem>();
        }

        public void AddStatusToItem(StatusItem statusItem)
        {
            StatusItems.Add(statusItem);
        }
    }
}
