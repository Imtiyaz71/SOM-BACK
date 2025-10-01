namespace Som_Models.VW_Models
{
    public class VM_Member
    {
        public int MemNo { get; set; }
        public string GivenName { get; set; }
        public string SureName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string NiD { get; set; }
        public string BiCNo { get; set; }
        public string PassportNo { get; set; }
        public string Nationality { get; set; }
        public int GenderId { get; set; }
        public string Gender { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
        public string Address { get; set; }
        public string IdenDocu { get; set; }   // varchar(max)
        public string Photo { get; set; }      // varchar(max)
        public string CreateDate { get; set; } // varchar(100)
        public string CreateBy { get; set; }
        public string UpdateDate { get; set; }
        public int compId { get; set; }
    }
}
