using Microsoft.AspNetCore.Mvc;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using System.Collections.Generic;
using H3CinemaBooking.Repository.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace H3CinemaBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IGenericRepository<Area> _areaRepository;

        public AreaController(IGenericRepository<Area> areaRepository)
        {
            _areaRepository = areaRepository;
        }

        // GET: api/<AreaController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> GetAll()
        {
            var areas = await _areaRepository.GetAllAsync();
            if (areas == null || areas.Count == 0)
            {
                return NoContent();
            }
            return Ok(areas);
        }

        // GET api/<AreaController>/5
        [HttpGet("{id}")]
        public ActionResult<Area> GetById(int id)
        {
            var area = _areaRepository.GetById(id);
            if (area == null)
            {
                return NotFound();
            }
            return Ok(area);
        }

        // POST api/<AreaController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Area> Post([FromBody] Area area)
        {
            if (area == null)
            {
                return BadRequest("Area data is required");
            }
            _areaRepository.Create(area);
            return Ok(area);
        }

        // PUT api/<AreaController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, [FromBody] Area area)
        {
            if (area == null)
            {
                return BadRequest();
            }

            var existingArea = _areaRepository.GetById(id);
            if (existingArea == null)
            {
                return NotFound();
            }

            existingArea.AreaName = area.AreaName;
            existingArea.RegionID = area.RegionID;

            _areaRepository.Update(existingArea);
            return Ok(existingArea);
        }

        // DELETE api/<AreaController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var area = _areaRepository.GetById(id);
            if (area == null)
            {
                return NotFound();
            }
            _areaRepository.DeleteById(id);
            return Ok(new { message = "Area deleted successfully" });
        }
    }
}
