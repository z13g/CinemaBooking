using Microsoft.AspNetCore.Mvc;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace H3CinemaBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IGenericRepository<Region> _regionRepository;

        public RegionController(IGenericRepository<Region> regionRepository)
        {
            _regionRepository = regionRepository;
        }

        // GET: api/<RegionController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Region>>> GetAll()
        {
            var regions = await _regionRepository.GetAllAsync();
            if (regions == null || regions.Count == 0)
            {
                return NoContent();
            }
            return Ok(regions);
        }

        // GET api/<RegionController>/5
        [HttpGet("{id}")]
        public ActionResult<Region> GetById(int id)
        {
            var region = _regionRepository.GetById(id);
            if (region == null)
            {
                return NotFound();
            }
            return Ok(region);
        }

        // POST api/<RegionController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Region> Post([FromBody] Region region)
        {
            if (region == null)
            {
                return BadRequest("Region data is required");
            }
            _regionRepository.Create(region);
            return Ok(region);
        }

        // PUT api/<RegionController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, [FromBody] Region region)
        {
            if (region == null)
            {
                return BadRequest();
            }

            var existingRegion = _regionRepository.GetById(id);
            if (existingRegion == null)
            {
                return NotFound();
            }

            existingRegion.RegionName = region.RegionName;

            _regionRepository.Update(existingRegion);
            return Ok(existingRegion);
        }

        // DELETE api/<RegionController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var region = _regionRepository.GetById(id);
            if (region == null)
            {
                return NotFound();
            }
            _regionRepository.DeleteById(id);
            return Ok(new { message = "Region deleted successfully" });
        }
    }
}
