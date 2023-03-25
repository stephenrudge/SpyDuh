using System.ComponentModel.DataAnnotations;

namespace SpyDuhLakers.Models;
public class User
{
    public int Id { get; set; }
    public int AgencyId { get; set; }
    
    [Required]
    public string Name { get; set; }    
    public List<Enemy>? Enemies { get; set; } = null;
    public List<Friend>? Friends { get; set; } = null;
    public List<Skill>? Skills { get; set; } = null;
    public List<Service>? Services { get; set; } = null;
  

}
