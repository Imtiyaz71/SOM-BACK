namespace Som_Models.Models
{
    public class Project
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }           // ProjectId
        public int CompId { get; set; }              // compId
        public string ProjectName { get; set; }      // ProjectName
        public string ProLocation { get; set; }      // ProLocation
        public float Budget { get; set; }            // Budget
        public int DiretorId { get; set; }           // DiretorId
        public string StartDate { get; set; }      // StartDate
        public string TentitiveEndDate { get; set; } // TentitiveEndDate
    }
}
