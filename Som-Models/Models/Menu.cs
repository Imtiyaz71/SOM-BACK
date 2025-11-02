namespace Som_Models.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public int? ParentId { get; set; } 
        public string MenuUrl { get; set; }
       

    }
}
