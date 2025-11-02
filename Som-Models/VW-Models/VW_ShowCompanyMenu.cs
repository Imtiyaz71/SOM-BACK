namespace Som_Models.VW_Models
{
    public class VW_ShowCompanyMenu
    {
        public int Id { get; set; }           // e.Id
        public int CompId { get; set; }       // e.compId
        public string CName { get; set; }     // com.cName
        public string Designation { get; set; } // au.Designation
        public int ParentId { get; set; }     // c.parentId
        public string ParentMenu { get; set; } // p.MenuName
        public int ChildId { get; set; }      // e.menuId
        public string ChildMenu { get; set; } // c.MenuName
        public string MenuUrl { get; set; }   // c.MenuUrl
    }
}
