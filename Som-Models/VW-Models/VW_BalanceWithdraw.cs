namespace Som_Models.VW_Models
{
    public class VW_BalanceWithdraw
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public int MemNo { get; set; }
        public int FProject { get; set; }
        public string ProjectName { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public string WDate { get; set; }
        public string WMonth { get; set; }
        public int WYear { get; set; }
        public string GivenName { get; set; }
        public string SureName { get; set; }
    }
}
