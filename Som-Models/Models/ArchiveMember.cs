namespace Som_Models.Models
{
    public class ArchiveMember
    {
        public int Id { get; set; }
        public int MemNo { get; set; }
        public string GivenName { get; set; }
        public string SureName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string NiD { get; set; }
        public string BiCNo { get; set; }
        public string PassportNo { get; set; }
        public string Nationality { get; set; }
        public int Gender { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
        public string Address { get; set; }
        public string IdenDocu { get; set; }   // varchar(max)
        public string Photo { get; set; }      // varchar(max)
        public string CreateDate { get; set; } // varchar(100)
        public string CreateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
