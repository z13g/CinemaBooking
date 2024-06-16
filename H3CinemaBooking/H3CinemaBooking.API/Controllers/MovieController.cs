using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace H3CinemaBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // GET: api/<MovieController>
        [HttpGet]
        public ActionResult<List<Movie>> GetAll()
        {
            var result = _movieRepository.GetAll();
            if (result == null || result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        // GET api/<MovieController>/id
        [HttpGet("{id}")]
        public ActionResult<Movie> GetByID(int id)
        {
            var movie = _movieRepository.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        // POST api/<MovieController>
        [HttpPost("Simple")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Movie> Post(Movie movie)
        {
            if (movie == null)
            {
                return BadRequest("Movie data is required");
            }
            _movieRepository.Create(movie);
            return Ok("Movie created successfully.");
        }

        [HttpPost("Complex")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Movie> PostComplex(Movie movie)
        {
            if (movie == null)
            {
                return BadRequest("Movie data is required");
            }

            try
            {
                // Initialize an empty list to hold all genre names from all genres
                List<string> allGenreNames = new List<string>();

                if (movie.Genres != null)
                {
                    foreach (var genre in movie.Genres)
                    {
                        allGenreNames.AddRange(genre.GenreName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                              .Select(gn => gn.Trim()));
                    }
                }

                // Clear genres to avoid processing existing references
                movie.Genres = null;

                var resultMovie = _movieRepository.CreateComplex(movie, allGenreNames);

                return Ok(resultMovie);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<MovieController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, [FromBody] Movie movie)
        {
            if (movie == null)
            {
                return BadRequest();
            }

            var existingMovie = _movieRepository.GetById(id);
            if (existingMovie == null)
            {
                return NotFound();
            }

            _movieRepository.UpdateByID(id, movie);
            return Ok(movie);
        }

        // DELETE api/<MovieController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            if (_movieRepository.DeleteByID(id))
            {
                return Ok(new { message = "Movie deleted successfully" });
            }
            return NotFound("ID was not found!");
        }
    }
}
