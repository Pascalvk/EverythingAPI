namespace EverythingAPI.Models
{
    public class StatusItem
    {
        // Database creates ID
        public int StatusItemId { get; set; }
        public string StatusName { get; set; }

        public StatusItem(int statusItemId, string statusName)
        {
            StatusItemId = statusItemId;
            StatusName = statusName;
        }
    }
}
