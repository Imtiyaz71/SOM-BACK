namespace Som_Models.VW_Models
{
    public class VW_BalanceSegment
    {
        public int Id { get; set; }
        public int compId { get; set; }
        public int Vendor { get; set; }
        public string vType { get; set; }
        public string Descri { get; set; }
        public double Amount { get; set; }
        public string createDate { get; set; }
    }
}
