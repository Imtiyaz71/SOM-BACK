namespace Som_Models.VW_Models
{
    public class VW_Journal
    {
        public int Years { get; set; }
        public string Months { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
    }
}
