using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpyDuhLakers.Models;
using SpyDuhLakers.Repositories;

namespace SpyDuhLakers.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SkillsController : ControllerBase
{
    private readonly ISkillRepository _skillRepository;

    public SkillsController(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }

    [HttpGet]
    public IActionResult GetAllSkills()
    {
        return Ok(_skillRepository.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var skill = _skillRepository.GetById(id);
        if (skill == null)
        {
            return NotFound();
        }
        return Ok(skill);
    }

    [HttpPost]
    public IActionResult AddSkill(Skill skill)
    {
        _skillRepository.Insert(skill);
        return Created("/api/skills/" + skill.Id, skill);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateSkill(int id, Skill skill)
    {
        if (id != skill.Id)
        {
            return BadRequest();
        }
        _skillRepository.Update(skill);
        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteEnemy(int id)
    {
        var enemy = _skillRepository.GetById(id);
        if (enemy == null)
        {
            return NotFound();
        }
        _skillRepository.Delete(enemy.Id);
        return NoContent();
    }

}
