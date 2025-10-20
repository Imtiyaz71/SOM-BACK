namespace Som_Models.VW_Models
{
    public class VW_ProjectWiseExpense
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public string ProjectInfo { get; set; }   // ProjectName (ProjectId)
        public string Purpose { get; set; }
        public decimal Amount { get; set; }
        public DateTime EDate { get; set; }
        public string EMonth { get; set; }
        public int EYear { get; set; }
        public string EBy { get; set; }
    }
}
