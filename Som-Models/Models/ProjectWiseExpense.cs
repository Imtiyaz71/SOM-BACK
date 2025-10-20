namespace Som_Models.Models
{
    public class ProjectWiseExpense
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public int ProjectId { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime EDate { get; set; }
        public string EMonth { get; set; }
        public string EYear { get; set; }
        public string EBy { get; set; }
    }
}
