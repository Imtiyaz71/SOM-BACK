namespace Som_Models.Models
{
    public class Advisory
    {
        public int Id { get; set; }          // Identity / Primary Key
        public int CompId { get; set; }      // Company ID
        public int MemNo { get; set; }       // Member Number
        public int AdRole { get; set; }      // Advisory Role ID
        public string Validity { get; set; } // Validity period
        public int CStatus { get; set; }     // Current status
    }
}
