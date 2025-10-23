namespace Som_Models.Models
{
    public class RevenueHistory
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public float Amount { get; set; }
        public string Descri { get; set; }   // varchar(max)
        public string Dates { get; set; }    // varchar(50)
        public string Months { get; set; }   // varchar(50)
        public int Years { get; set; }
    }
}
