using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IMemberService
    {
        public Task<List<VM_Member>> Getmember();
        public Task<VM_Member> GetmemberById(int Memno);
        public Task<string> SaveMember(Members model);
    }
}
