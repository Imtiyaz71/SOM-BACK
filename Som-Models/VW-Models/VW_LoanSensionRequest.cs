namespace Som_Models.VW_Models
{
    public class VW_LoanSensionRequest
    {
        public int CompId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BAddress { get; set; }
        public string NId { get; set; }
        public string DOB { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
        public string Photo { get; set; }

        // LoanSension info
        public int LoanType { get; set; }
        public double Amount { get; set; }
        public string SDate { get; set; }
        public string SMonth { get; set; }
        public int SYear { get; set; }

        // SomityTransection info
        public string SBy { get; set; }
    }
}
