namespace Som_Models.VW_Models
{
    public class VW_AdvisoryList
    {
        public int Id { get; set; }          // ad.Id
        public int CompId { get; set; }      // ad.CompId
        public int MemNo { get; set; }       // ad.MemNo
        public string GivenName { get; set; } // m.GivenName
        public string SureName { get; set; }  // m.SureName
        public int AdRole { get; set; }      // ad.AdRole
        public string Roles { get; set; }    // r.Roles
        public string Validity { get; set; } // ad.Validity
        public int CStatus { get; set; }     // ad.CStatus
    }
}
