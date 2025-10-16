namespace Som_Models.VW_Models
{
    public class VW_MemberProjectAccount
    {
        public int Id { get; set; }
        public int projectId { get; set; }
        public int memNo { get; set; }
        public int compId { get; set; }
        public int Amount { get; set; }
        public string createDate { get; set; }
        public string updateDate { get; set; }
        public string GivenName { get; set; }
        public string SureName { get; set; }

        public string ProjectName { get; set; }
    }
}
