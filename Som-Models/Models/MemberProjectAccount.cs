namespace Som_Models.Models
{
    public class MemberProjectAccount
    {
        public int Id { get; set; }
        public int projectId { get; set; }
        public int memNo { get; set; }
        public int compId { get; set; }
        public int Amount { get; set; }
        public string createDate { get; set; }
        public string updateDate { get; set; }
    }
}
