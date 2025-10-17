namespace Som_Models.VW_Models
{
    public class VW_MemberBalance
    {
        public int Sl { get; set; }
        public string MemberInfo { get; set; } = string.Empty;
        public string? ProjectInfo { get; set; }
        public decimal? ProjectBalance { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
