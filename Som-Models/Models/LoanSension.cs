namespace Som_Models.Models
{
    public class LoanSension
    {
        public int Id { get; set; }
        public int brwId { get; set; }
        public int loanType { get; set; }
        public double Amount { get; set; }
        public string sDate { get; set; }
        public string sMonth { get; set; }
        public int sYear { get; set; }
    }
}
