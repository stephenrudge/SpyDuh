using Microsoft.AspNetCore.Mvc;
using SpyDuhLakers.Models;
using SpyDuhLakers.Repositories;
using System.Collections.Generic;

namespace SpyDuhLakers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnemiesController : ControllerBase
    {
        private readonly IEnemyRepository _enemyRepository;

        public EnemiesController(IEnemyRepository enemyRepository)
        {
            _enemyRepository = enemyRepository;
        }

        [HttpPost]
        public IActionResult Post(Enemy enemy)
        {
            _enemyRepository.Insert(enemy);
            return Created("/api/Enemies/" + enemy.Id , enemy);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEnemy(int id, Enemy enemy)
        {
            if (id != enemy.Id)
            {
                return BadRequest();
            }
            _enemyRepository.Update(enemy);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEnemy(int id)
        {
            var enemy = _enemyRepository.GetById(id);
            if (enemy == null)
            {
                return NotFound();
            }
            _enemyRepository.Delete(enemy.Id);
            return NoContent();
        }
    }
}
