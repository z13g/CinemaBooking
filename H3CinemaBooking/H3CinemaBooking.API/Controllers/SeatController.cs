using Microsoft.AspNetCore.Mvc;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace H3CinemaBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatRepository _seatRepository;
        public SeatController(ISeatRepository seatRepository) => _seatRepository = seatRepository;

        // GET: api/<SeatController>
        [HttpGet]
        public ActionResult<List<Seat>> GetAll()
        {
            var result = _seatRepository.GetAll();
            if (result == null || result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        // GET api/<SeatController>/id
        [HttpGet("{id}")]
        public ActionResult<Seat> GetByID(int id)
        {
            var seat = _seatRepository.GetById(id);
            if (seat == null)
            {
                return NotFound();
            }
            return Ok(seat);
        }

        // POST api/<SeatController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Seat> Post([FromBody] Seat seat)
        {
            if (seat == null)
            {
                return BadRequest("Seat data is required");
            }
            _seatRepository.Create(seat);
            return Ok(seat);
        }

        // POST api/<SeatController>/bulk
        [HttpPost("bulk")]
        [Authorize(Roles = "Admin")]
        public IActionResult PostBulk([FromBody] List<Seat> seats)
        {
            if (seats == null || seats.Count == 0)
            {
                return BadRequest("Seat data is required");
            }
            _seatRepository.CreateBulk(seats);
            return Ok(seats);
        }

        // DELETE api/<SeatController>/ID
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var seat = _seatRepository.GetById(id);
            if (seat == null)
            {
                return NotFound();
            }
            _seatRepository.DeleteByID(id);
            return Ok(new { message = "Seat deleted successfully" });
        }

        // PUT api/<SeatController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update([FromBody] Seat seat)
        {
            if (seat == null)
            {
                return BadRequest();
            }

            try
            {
                var updatedSeat = _seatRepository.UpdateByID(seat.SeatID, seat);
                return Ok(updatedSeat);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
