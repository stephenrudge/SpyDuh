using SpyDuhLakers.Models;

namespace SpyDuhLakers.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserbyId(int Id);
        void Insert(User user);
       List<User> GetUserByAgency(string agency);
        List<User> GetUserBySkill(string name);
    }
}