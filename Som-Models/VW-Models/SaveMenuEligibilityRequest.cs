namespace Som_Models.VW_Models
{
    public class SaveMenuEligibilityRequest
    {
        public int CompId { get; set; }
        public int RoleId { get; set; }
        public List<int> MenuIds { get; set; }
    }
}
