namespace Som_Models.VW_Models
{
    public class VW_ClientStatus
    {
        public int Id { get; set; }          // ClientStatus Id
        public string CName { get; set; }    // Company Name
        public DateTime? PDate { get; set; } // ClientStatus PDate
        public DateTime? EDate { get; set; } // ClientStatus EDate
        public int CStatus { get; set; }     // ClientStatus CStatus
    }
}
