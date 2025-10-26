namespace Som_Models.Models
{
    public class Meeting
    {
        public int Id { get; set; }               // Identity primary key
        public int CompId { get; set; }           // Company ID
        public string Title { get; set; }         // Meeting title
        public string Biboroni { get; set; }      // Description
        public string MeetingDate { get; set; }   // varchar(50) -> date as string
        public string MeetingMonth { get; set; }  // varchar(50)
        public int MeetingYear { get; set; }      // int
    }
}
