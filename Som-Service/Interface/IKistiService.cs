using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IKistiService
    {
        public Task<List<Cr>> GetCrData();
        public Task<List<VM__KistiTypes>> GetKistiTypes(int compId);
        public Task<List<VM__KistiTypes>> GetKistiTypesByProject(int compId, int projectid);
        public Task<VM__KistiTypes> GetKistiTypesById(int id);
        public Task<string> SaveKistiType(KistiTypes k);
    }
}
