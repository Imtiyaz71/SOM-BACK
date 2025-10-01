using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IMemberService
    {
        public Task<List<VM_Member>> Getmember(int compId);
        public Task<VM_Member> GetmemberById(int Memno);
        public Task<List<ArchiveMember>> GetArchivemember(int compId);
        public Task<List<MemberTransferLogs>> TransferLogsList(int compId);
        public Task<string> SaveMember(Members model);
        public Task<string> EditMember(Members model);
        public Task<string> TransferMember(Members model);
        public Task<string> MemberDeactivation(int memNo,int compId,string entryBy);
        public Task<List<VW_DeactiveLogs>> DeactiveLogs(int compId);
    }
}
