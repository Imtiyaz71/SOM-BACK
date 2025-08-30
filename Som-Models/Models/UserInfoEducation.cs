namespace Som_Models.Models
{
    public class UserInfoEducation
    {
        public int ID { get; set; }
        public string? Degree { get; set; }
        public string? FieldOfStudy { get; set; }
        public string? SchoolName { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Description { get; set; }
        public string Username { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        // Navigation property

    }
}
