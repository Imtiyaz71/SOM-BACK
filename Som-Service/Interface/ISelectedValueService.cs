using Som_Models.Models;

namespace Som_Service.Interface
{
    public interface ISelectedValueService
    {
        Task<List<Designation>> GetSelectedDesignation();
        Task<List<Authorizer>> GetSelectedAuthorizer();
        Task<List<Gender>> GetGender();
        Task<List<AdvisoryActivation>> GetActivation();

    }
}
