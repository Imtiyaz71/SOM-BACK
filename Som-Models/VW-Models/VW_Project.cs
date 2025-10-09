namespace Som_Models.VW_Models
{
    public class VW_Project
    {
        public int Id { get; set; }                // p.Id
        public int ProjectId { get; set; }         // p.ProjectId
        public string ProjectName { get; set; }    // p.ProjectName
        public string ProLocation { get; set; }    // p.ProLocation
        public float Budget { get; set; }          // p.Budget
        public int DiretorId { get; set; }         // p.DiretorId
        public string GivenName { get; set; }      // m.GivenName
        public string SureName { get; set; }       // m.SureName
        public string StartDate { get; set; }    // p.StartDate
        public string TentitiveEndDate { get; set; } // p.TentitiveEndDate
    }
}
