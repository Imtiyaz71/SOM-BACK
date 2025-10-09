namespace Som_Models.VW_Models
{
    public class VW_AssignProject
    {
        public int Id { get; set; }              // pa.Id
        public int MemNo { get; set; }           // pa.memNo
        public int projectid { get; set; }
        public string projectname { get; set; }
        public string GivenName { get; set; }    // m.GivenName
        public string SureName { get; set; }     // m.SureName
        public int CompId { get; set; }          // pa.compId
        public string AssignDate { get; set; } // pa.assignDate
        public string AssignBy { get; set; }     // pa.assignBy
    }
}
