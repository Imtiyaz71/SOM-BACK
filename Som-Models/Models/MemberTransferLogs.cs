namespace Som_Models.Models
{
    public class MemberTransferLogs
    {
        public int Id { get; set; }
        public int memno { get; set; }

        public string FromMember { get; set; }
        public string ToMember { get; set; }
        public string FromNid { get; set; }
        public string ToNid { get; set; }
        public string FromBiCNo { get; set; }
        public string ToBiC { get; set; }
        public string TransferDate { get; set; }
        public string TransferBy { get; set; }
        public int compId { get; set; }
    }
}
