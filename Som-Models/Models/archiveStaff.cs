namespace Som_Models.Models
{
    public class archiveStaff
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public string FullName { get; set; }
        public string NId { get; set; }
        public string FullAddress { get; set; }
        public int StaffType { get; set; }
        public string Photo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DeactiveDate { get; set; }
    }
}
