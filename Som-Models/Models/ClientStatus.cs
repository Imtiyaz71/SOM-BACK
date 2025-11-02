namespace Som_Models.Models
{
    public class ClientStatus
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime? PDate { get; set; }
        public DateTime? EDate { get; set; }
        public int CStatus { get; set; } // 0 = expired/today, 1 = active
    }
}
