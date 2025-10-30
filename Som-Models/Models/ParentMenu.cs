namespace Som_Models.Models
{
    public class ParentMenu
    {
        public int Id { get; set; }           // Primary key, identity
        public string MenuName { get; set; }  // Menu name
        public int SortOrder { get; set; }    // Sort order
    }
}
