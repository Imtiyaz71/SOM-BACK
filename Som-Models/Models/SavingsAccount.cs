namespace Som_Models.Models
{
    public class SavingsAccount
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public string AccountName { get; set; }
        public string Organization { get; set; }
        public string AccountNo { get; set; }
        public string Branch { get; set; }
        public decimal Balance { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
    }
}
