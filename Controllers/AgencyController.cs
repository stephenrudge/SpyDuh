using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpyDuhLakers.Models;
using SpyDuhLakers.Repositories;

namespace SpyDuhLakers.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AgenciesController : ControllerBase
{
    private readonly IAgencyRepository _agencyRepository;


    public AgenciesController(IAgencyRepository agencyRepository)
    {
        _agencyRepository = agencyRepository;
    }

    [HttpGet]
    public IActionResult GetAllAgencies()
    {
        return Ok(_agencyRepository.GetAllAgencies());
    }

    [HttpGet("{id}")]
    public IActionResult GetAgencyById(int id)
    {
        var agency = _agencyRepository.GetAgencyById(id);
        if (agency == null)
        {
            return NotFound();
        }
        return Ok(agency);
    }

    [HttpPost]
    public IActionResult AddAgency(Agency agency)
    {
        _agencyRepository.Insert(agency);
        return Created("/api/agency/" + agency.Id, agency);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateAgency(int id, Agency agency)
    {
        if (id != agency.Id)
        {
            return BadRequest();
        }
        _agencyRepository.Update(agency);
        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteAgency(int id)
    {
        var agency = _agencyRepository.GetAgencyById(id);
        if (agency == null)
        {
            return NotFound();
        }
        _agencyRepository.Delete(agency.Id);

        return NoContent();
    }

}
