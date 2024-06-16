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
    public class CinemaController : ControllerBase
    {
        private readonly IGenericRepository<Cinema> _cinemaRepository;

        public CinemaController(IGenericRepository<Cinema> cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        // GET: api/<CinemaController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cinema>>> GetAll()
        {
            var cinemas = await _cinemaRepository.GetAllAsync();
            if (cinemas == null || cinemas.Count == 0)
            {
                return NoContent();
            }
            return Ok(cinemas);
        }

        // GET api/<CinemaController>/5
        [HttpGet("{id}")]
        public ActionResult<Cinema> GetById(int id)
        {
            var cinema = _cinemaRepository.GetById(id);
            if (cinema == null)
            {
                return NotFound();
            }
            return Ok(cinema);
        }

        // POST api/<CinemaController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Cinema> Post([FromBody] Cinema cinema)
        {
            if (cinema == null)
            {
                return BadRequest("Cinema data is required");
            }
            _cinemaRepository.Create(cinema);
            return Ok(cinema);
        }

        // PUT api/<CinemaController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, [FromBody] Cinema cinema)
        {
            if (cinema == null)
            {
                return BadRequest();
            }

            var existingCinema = _cinemaRepository.GetById(id);
            if (existingCinema == null)
            {
                return NotFound();
            }

            existingCinema.Name = cinema.Name;
            existingCinema.Location = cinema.Location;
            existingCinema.NumberOfHalls = cinema.NumberOfHalls;
            existingCinema.AreaID = cinema.AreaID;

            _cinemaRepository.Update(existingCinema);
            return Ok(existingCinema);
        }

        // DELETE api/<CinemaController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var cinema = _cinemaRepository.GetById(id);
            if (cinema == null)
            {
                return NotFound();
            }
            _cinemaRepository.DeleteById(id);
            return Ok(new { message = "Cinema deleted successfully" });
        }
    }
}
