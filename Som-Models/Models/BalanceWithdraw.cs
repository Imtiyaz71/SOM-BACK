namespace Som_Models.Models
{
    public class BalanceWithdraw
    {
        public int Id { get; set; }
        public int compId { get; set; }
        public int memNo { get; set; }
        public int fProject { get; set; }
        public double amount { get; set; }
        public string remarks { get; set; }
        public string wDate { get; set; }
        public string wMonth { get; set; }
        public int wYear { get; set; }
        public string wBy { get; set; }
    }
}
