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
    public class CinemaHallController : ControllerBase
    {
        private readonly IGenericRepository<CinemaHall> _cinemaHallRepository;

        public CinemaHallController(IGenericRepository<CinemaHall> cinemaHallRepository)
        {
            _cinemaHallRepository = cinemaHallRepository;
        }

        // GET: api/<CinemaHallController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CinemaHall>>> GetAll()
        {
            var cinemaHalls = await _cinemaHallRepository.GetAllAsync();
            if (cinemaHalls == null || cinemaHalls.Count == 0)
            {
                return NoContent();
            }
            return Ok(cinemaHalls);
        }

        // GET api/<CinemaHallController>/id
        [HttpGet("{id}")]
        public ActionResult<CinemaHall> GetByID(int id)
        {
            var cinemaHall = _cinemaHallRepository.GetById(id);
            if (cinemaHall == null)
            {
                return NotFound();
            }
            return Ok(cinemaHall);
        }

        // POST api/<CinemaHallController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<CinemaHall> Post([FromBody] CinemaHall cinemaHall)
        {
            if (cinemaHall == null)
            {
                return BadRequest("CinemaHall data is required");
            }
            _cinemaHallRepository.Create(cinemaHall);
            return Ok(cinemaHall);
        }

        // PUT api/<CinemaHallController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, [FromBody] CinemaHall cinemaHall)
        {
            if (cinemaHall == null)
            {
                return BadRequest();
            }

            var existingCinemaHall = _cinemaHallRepository.GetById(id);
            if (existingCinemaHall == null)
            {
                return NotFound();
            }

            existingCinemaHall.HallName = cinemaHall.HallName;
            existingCinemaHall.CinemaID = cinemaHall.CinemaID;

            _cinemaHallRepository.Update(existingCinemaHall);
            return Ok(existingCinemaHall);
        }

        // DELETE api/<CinemaHallController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var cinemaHall = _cinemaHallRepository.GetById(id);
            if (cinemaHall == null)
            {
                return NotFound();
            }
            _cinemaHallRepository.DeleteById(id);
            return Ok(new { message = "CinemaHall deleted successfully" });
        }
    }
}
