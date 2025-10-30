namespace Som_Models.Models
{
    public class ChildMenu
    {
        public int Id { get; set; }           // Primary key, identity
        public int ParentId { get; set; }     // Foreign key to ParentMenus
        public string MenuName { get; set; }  // Child menu name
        public string MenuUrl { get; set; }   // URL of the child menu
    }
}
