using Microsoft.AspNetCore.Mvc;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using System.Security.Cryptography;
using H3CinemaBooking.Repository.Repositories;
using H3CinemaBooking.Repository.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace H3CinemaBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly IShowRepository _showRepository;
        private readonly IShowService _showService;
        public ShowController(IShowRepository showRepository, IShowService showService) { 
            _showRepository = showRepository; 
            _showService = showService;
        }

        //TODO: Make a Get all here
        // GET: api/<ShowController>
        [HttpGet]
        public ActionResult<List<Show>> GetAll()
        {
            var result = _showRepository.GetAll();
            if (result == null || result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        // GET api/<ShowController>/id
        [HttpGet("{id}")]
        public ActionResult<Show> GetByID(int id)
        {
            var show = _showRepository.GetById(id);
            if (show == null)
            {
                return NotFound();
            }
            return Ok(show);
        }

        [HttpGet("bookInfo/{id}")]
        public ActionResult<BookShow> Get(int id)
        {
            var bookShow = _showService.SetBookShowObjekt(id);
            if (bookShow == null)
                return NotFound();
            return Ok(bookShow);
        }
 

        // POST api/<Showtroller>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Show>Post(Show show)
        {   
            if (show == null)
            {
                return BadRequest("Show data is required");
            }
            _showRepository.Create(show);
            return Ok("Show created successfully");
        }

        // DELETE api/<CinemaHallController>/ID
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            if (_showRepository.DeleteByID(id))
            {
                return Ok(new { message = "Show deleted successfully" });
            }
            return NotFound("ID was not found!");
        }

        //Update Api Movie with Genre
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Show show)
        {
            if (show == null)
            {
                return BadRequest();
            }
            var existingShow = _showRepository.GetById(id);
            if (existingShow == null)
            {
                return NotFound();
            }

            _showRepository.UpdateByID(id, show);
            return Ok(show);
        }

        [HttpGet("filtered")]
        public ActionResult<Dictionary<string, Dictionary<DateTime, List<ShowDetailsDTO>>>> GetFilteredShowsByArea(int areaId, int movieId)
        {
            try
            {
                var shows = _showService.GetFilteredShowsByCinemaAndDate(areaId, movieId);
                if (shows == null || shows.Count == 0)
                    return NotFound("No shows available for the specified area.");
                return Ok(shows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


    }
}
