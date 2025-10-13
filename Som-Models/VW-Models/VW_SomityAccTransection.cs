namespace Som_Models.VW_Models
{
    public class VW_SomityAccTransection
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public string Purpose { get; set; }
        public double Amount { get; set; }
        public int CrType { get; set; }
        public string CrName { get; set; }        // From join with cr table
        public string Remarks { get; set; }
        public string Dates { get; set; }
        public string Months { get; set; }
        public int Years { get; set; }
        public string TransectionBy { get; set; }
    }
}
