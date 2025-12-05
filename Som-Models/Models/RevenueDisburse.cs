namespace Som_Models.Models
{
    public class RevenueDisburse
    {
        public int Id { get; set; }
        public int compId { get; set; }
        public decimal CurrRev { get; set; }
        public decimal DisRev { get; set; }
        public decimal Avail { get; set; }
        public string createDate { get; set; }
        public string CreateMonth { get; set; }
        public int CreateYear { get; set; }
        public string CreateBy { get; set; }
    }
}
