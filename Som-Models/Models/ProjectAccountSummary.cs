namespace Som_Models.Models
{
    public class ProjectAccountSummary
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public int ProjectId { get; set; }
        public decimal Budget { get; set; }
        public decimal Balance { get; set; }
        public decimal Expense { get; set; }
        public string LastTransaction { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
