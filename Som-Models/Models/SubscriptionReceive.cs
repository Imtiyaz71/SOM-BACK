namespace Som_Models.Models
{
    public class SubscriptionReceive
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int compId { get; set; }
        public int memNo { get; set; }
        public double PaybleAmount { get; set; }
        public double RecAmount { get; set; }
        public double Due { get; set; }
        public string Remarks { get; set; }
        public string RecDate { get; set; }
        public string RecMonth { get; set; }
        public int RecYear { get; set; }
    }
}
