namespace Som_Models.VW_Models
{
    public class VW_KistiRecHistory
    {
        public int Year { get; set; }           // RecYear
        public int Project { get; set; }        // projectId

        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }

        public decimal Total { get; set; }     // Sum of all months
    }
}
