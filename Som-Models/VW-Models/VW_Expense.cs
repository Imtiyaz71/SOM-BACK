namespace Som_Models.VW_Models
{
    public class VW_Expense
    {
        public int Id { get; set; }
        public int exType { get; set; }
        public string TypeName { get; set; }
        public int compId { get; set; }
        public double amount { get; set; }
        public string Descri { get; set; }

        public string eDate { get; set; }
        public string eMonth { get; set; }
        public string eBy { get; set; }
        public int eYear { get; set; }
    }
}
