using SpyDuhLakers.Models;

namespace SpyDuhLakers.Repositories
{
    public interface IAgencyRepository
    {
        void Delete(int id);
        List<Agency> GetAllAgencies();
        List<User> GetUserByAgency(string agency);
        Agency GetAgencyById(int id);
        void Insert(Agency agency);
        void Update(Agency agency);
    }
}