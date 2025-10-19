namespace Som_Models.VW_Models
{
    public class VW_Staff
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public string FullName { get; set; }
        public string NId { get; set; }
        public string FullAddress { get; set; }
        public int? StaffType { get; set; }  // FK to StaffDesignation
        public string Designation { get; set; } // from staffdesignation
        public string Photo { get; set; }        // path to uploaded photo
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
