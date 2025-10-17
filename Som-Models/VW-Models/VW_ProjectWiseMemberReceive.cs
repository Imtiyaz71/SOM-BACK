namespace Som_Models.VW_Models
{
    public class VW_ProjectWiseMemberReceive
    {
        public string MemberInfo { get; set; } = string.Empty;    // GivenName + SureName + memNo
        public string ProjectInfo { get; set; } = string.Empty;   // ProjectName + ProjectId
        public string TypeName { get; set; } = string.Empty;      // Kisti Type
        public int RecYear { get; set; }                           // Year
        public string Jan { get; set; } = string.Empty;           // Payble-Rec-Due format
        public string Feb { get; set; } = string.Empty;
        public string Mar { get; set; } = string.Empty;
        public string Apr { get; set; } = string.Empty;
        public string May { get; set; } = string.Empty;
        public string Jun { get; set; } = string.Empty;
        public string Jul { get; set; } = string.Empty;
        public string Aug { get; set; } = string.Empty;
        public string Sep { get; set; } = string.Empty;
        public string Oct { get; set; } = string.Empty;
        public string Nov { get; set; } = string.Empty;
        public string Dec { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}
