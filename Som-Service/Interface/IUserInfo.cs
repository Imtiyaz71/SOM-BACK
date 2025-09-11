using Som_Models.Models;
using Som_Models.VW_Models;
namespace Som_Service.Interface
{
    public interface IUserInfo
    {
        Task<List<UserInfoBasic>> GetUserInfoBasic();
        Task<UserInfoBasic> GetUserInfoBasicByUser(string Username);
        Task<VW_UserInfo> GetUserInfo(string Username);
    //    Task<UserPhoto> GetUserPhoto(string Username);
        Task<string> SaveUserInfoBasic(UserInfoBasic user);
        Task<string> EditUserInfo(IFormFile file, VW_UserInfo user);
        //Task<string> DeleteUserInfoBasic(string Username);
        //Task<UserInfoEducation> GetUserInfoEducation();
        Task<UserInfoEducation> GetUserInfoEducationByUser(string Username);
        Task<string> SaveUserInfoEducation(UserInfoEducation user);
        //Task<string> EditUserInfoEducation(string Username, UserInfoEducation user);
        Task<string> DeleteAllUserInfo(string Username);
        Task<string> SaveUserPhoto(IFormFile file,UserPhoto user);
        Task<string> UserMap(Mapper map);
        Task<string> DeleteUser(string username,string deleteby);
        Task<List<VW_MapperDetails>> GetMapdetails();

    }
}
