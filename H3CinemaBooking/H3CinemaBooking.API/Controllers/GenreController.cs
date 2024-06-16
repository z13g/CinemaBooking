using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace H3CinemaBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _movieGenreRepository;

        public GenreController(IGenreRepository movieGenreRepository)
        {
            _movieGenreRepository = movieGenreRepository;
        }

        // GET: api/<GenreController>
        [HttpGet]
        public ActionResult<List<Genre>> GetAll()
        {
            var result = _movieGenreRepository.GetAll();
            if (result == null || result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        // GET api/<GenreController>/id
        [HttpGet("{id}")]
        public ActionResult<Genre> GetByID(int id)
        {
            var genre = _movieGenreRepository.GetById(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        // POST api/<GenreController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Genre> Post([FromBody] Genre genre)
        {
            if (genre == null)
            {
                return BadRequest("Genre data is required");
            }
            _movieGenreRepository.Create(genre);
            return Ok(genre);
        }

        // PUT api/<GenreController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, [FromBody] Genre genre)
        {
            if (genre == null)
            {
                return BadRequest();
            }

            var existingGenre = _movieGenreRepository.GetById(id);
            if (existingGenre == null)
            {
                return NotFound();
            }

            existingGenre.GenreName = genre.GenreName;

            _movieGenreRepository.UpdateByID(id, genre);
            return Ok(existingGenre);
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var genre = _movieGenreRepository.GetById(id);
            if (genre == null)
            {
                return NotFound();
            }
            _movieGenreRepository.DeleteGenreByID(id);
            return Ok(new { message = "Genre deleted successfully" });
        }
    }
}
