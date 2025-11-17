namespace Som_Models.VW_Models
{
    public class VM_AccountOperation
    {
        public int CompId { get; set; }
        public int ParentId { get; set; }
        public int TType { get; set; }         // 1 = Deposit, 0 = Withdraw
        public decimal Amount { get; set; }
        public string Dates { get; set; }
        public string CreateBy { get; set; }
    }
}
