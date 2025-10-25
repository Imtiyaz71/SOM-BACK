namespace Som_Models.VW_Models
{
    public class VW_RegularSubscription
    {
        public int Id { get; set; }
        public int compId { get; set; }
        public int memNo { get; set; }
        public string GivenName { get; set; }
        public string SureName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double paybleAmount { get; set; }
        public double recAmount { get; set; }
        public double Due { get; set; }
        public string RecDate { get; set; }
        public string RecMonth { get; set; }
        public string RecYear { get; set; }
        public string RecBy { get; set; }
    }
}
