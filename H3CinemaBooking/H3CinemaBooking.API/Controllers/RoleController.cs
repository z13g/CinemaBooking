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
    public class RoleController : ControllerBase
    {
        private readonly IGenericRepository<Roles> _roleRepository;

        public RoleController(IGenericRepository<Roles> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // GET: api/<RoleController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Roles>>> GetAll()
        {
            var roles = await _roleRepository.GetAllAsync();
            if (roles == null || roles.Count == 0)
            {
                return NoContent();
            }
            return Ok(roles);
        }

        // GET api/<RoleController>/5
        [HttpGet("{id}")]
        public ActionResult<Roles> GetById(int id)
        {
            var role = _roleRepository.GetById(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        // POST api/<RoleController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Roles> Post([FromBody] Roles role)
        {
            if (role == null)
            {
                return BadRequest("Role data is required");
            }
            _roleRepository.Create(role);
            return Ok(role);
        }

        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, [FromBody] Roles role)
        {
            if (role == null)
            {
                return BadRequest();
            }

            var existingRole = _roleRepository.GetById(id);
            if (existingRole == null)
            {
                return NotFound();
            }

            existingRole.RoleName = role.RoleName;

            _roleRepository.Update(existingRole);
            return Ok(existingRole);
        }

        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var role = _roleRepository.GetById(id);
            if (role == null)
            {
                return NotFound();
            }
            _roleRepository.DeleteById(id);
            return Ok(new { message = "Role deleted successfully" });
        }
    }
}
