using Microsoft.AspNetCore.Mvc;
using SpyDuhLakers.Models;
using SpyDuhLakers.Repositories;
using System.Collections.Generic;

namespace SpyDuhLakers.Controllers
{
    [ApiController]
    [Route("api/Services")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        // GET api/services
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_serviceRepository.GetAll());
        }

        // GET api/services/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Service service = _serviceRepository.GetById(id);
            if (service == null)
            {
                return NotFound();
            }
            return Ok(service);
        }

        // POST api/services
        [HttpPost]
        public IActionResult Post([FromBody] Service service)
        {
            if (service == null)
            {
                return BadRequest();
            }
            _serviceRepository.Insert(service);
            return Created("/api/services/" + service.Id, service);
        }

        // PUT api/services/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Service service)
        {
            if (service == null || service.Id != id)
            {
                return BadRequest();
            }

            Service existingService = _serviceRepository.GetById(id);
            if (existingService == null)
            {
                return NotFound();
            }

            existingService.UserId = service.UserId;
            existingService.Name = service.Name;

            _serviceRepository.Update(existingService);

            return NoContent();
        }

        // DELETE api/services/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Service service = _serviceRepository.GetById(id);
            if (service == null)
            {
                return NotFound();
            }

            _serviceRepository.Delete(service.Id);

            return NoContent();
        }
    }
}
