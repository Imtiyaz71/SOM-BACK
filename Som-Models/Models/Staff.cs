namespace Som_Models.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public string FullName { get; set; }
        public string NId { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string FullAddress { get; set; }
        public int? StaffType { get; set; }  // Nullable if not always required
        public string Photo { get; set; }
        public string CreateDate { get; set; }
        public string? UpdateDate { get; set; }  // Nullable because it may be updated later
        public string CreateBy { get; set; }
    }
}
