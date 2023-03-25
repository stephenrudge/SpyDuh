using SpyDuhLakers.Models;

namespace SpyDuhLakers.Repositories
{
    public interface ISkillRepository
    {
        List<Skill> GetAll();
        void Delete(int id);
        Skill GetById(int Id);
        void Insert(Skill enemy);
        void Update(Skill enemy);
       
    }
}