using SpyDuhLakers.Models;

namespace SpyDuhLakers.Repositories
{
    public interface IServiceRepository
    {
        List<Service> GetAll();
        Service GetById(int Id);
        void Insert(Service service);
        void Update(Service service);
        void Delete(int Id);
    }
}