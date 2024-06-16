using Microsoft.AspNetCore.Mvc;
using H3CinemaBooking.Repository.Service;
using H3CinemaBooking.Repository.Models;
using System.Security.Cryptography;
using System.Collections.Generic;
using H3CinemaBooking.Repository.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using H3CinemaBooking.Repository.Models.DTO;
using H3CinemaBooking.Repository.Repositories;
using H3CinemaBooking.Repository.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace H3CinemaBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailController : ControllerBase

    {

        private readonly IUserDetailService _userDetailService;

        public UserDetailController(IUserDetailService userDetailService)
        {
            _userDetailService = userDetailService;
        }


        [HttpGet]
        public ActionResult<List<UserDetail>> GetAll()
        {
            var result = _userDetailService.GetAllUserDetail();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDetail> GetByID(int id)
        {
            var userdetail = _userDetailService.GetUserDetailById(id);
            if (userdetail == null)
            {
                return NotFound();
            }
            return Ok(userdetail);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserDetail> Post(UserDetail userDetail)
        {
            var (hash, salt) = _userDetailService.CreateUserDetail(userDetail);
            if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(salt))
            {
                return BadRequest("Failed to create customer due to password hashing failure.");
            }
            return Ok(userDetail);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _userDetailService.DeleteUserdetail(id);
            return Ok();
        }

        [HttpPost("register")]
        public ActionResult<UserDetail> RegisterUser(RegisterUserDTO registerUserDetail)
        {
            List<string> errors = _userDetailService.ValidateUserInput(registerUserDetail);
            if (errors.Any())
            {
                return BadRequest(string.Join(", ", errors));
            }
            if (_userDetailService.CheckIfUserExistsFromEmail(registerUserDetail.Email) || _userDetailService.CheckIfUserExistsFromNumber(registerUserDetail.PhoneNumber))
            {
                return BadRequest("Email or Phonenumber already exists");
            }
            var jwt = _userDetailService.RegisterUser(registerUserDetail);
            if (jwt == "false")
            {
                return BadRequest("Something went wrong doing signup");
            }
            return Ok(jwt);
        }

        [HttpPost("registerAdminOutToken")]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserDetail> RegisterAdminWithOutToken(RegisterUserDTO registerUserDetail)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            List<string> errors = _userDetailService.ValidateUserInput(registerUserDetail);
            if (errors.Any())
            {
                return BadRequest(string.Join(", ", errors));
            }
            if (_userDetailService.CheckIfUserExistsFromEmail(registerUserDetail.Email) || _userDetailService.CheckIfUserExistsFromNumber(registerUserDetail.PhoneNumber))
            {
                return BadRequest("Email or Phonenumber already exists");
            }
            var result = _userDetailService.RegisterAdminWithoutToken(registerUserDetail);
            
            return Ok(result);
        }

        [HttpPost("registerCostumerOutToken")]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserDetail> RegisterCostumerWithOutToken(RegisterUserDTO registerUserDetail)
        {
            List<string> errors = _userDetailService.ValidateUserInput(registerUserDetail);
            if (errors.Any())
            {
                return BadRequest(string.Join(", ", errors));
            }
            if (_userDetailService.CheckIfUserExistsFromEmail(registerUserDetail.Email) || _userDetailService.CheckIfUserExistsFromNumber(registerUserDetail.PhoneNumber))
            {
                return BadRequest("Email or Phonenumber already exists");
            }
            var result = _userDetailService.RegisterCostumerWithoutToken(registerUserDetail);

            return Ok(result);
        }

        [HttpPost("login")]
        public ActionResult<UserDetail> Login(LoginUserDTO loginUserDetail)
        {
            List<string> errors = _userDetailService.ValidateUserInput(loginUserDetail);
            if (errors.Any())
            {
                return BadRequest(string.Join(", ", errors));
            }
            if (_userDetailService.CheckIfUserExistsFromEmail(loginUserDetail.Email))
            {
                var jwt = _userDetailService.Login(loginUserDetail);
                if (jwt != "False")
                {
                    return Ok(jwt);
                }
                return BadRequest("Invalid credentials");

            }
            return BadRequest("Invalid credentials");
        }


        //Update 
        [HttpPut("{id}")]
        public ActionResult Update(UserDetailDTO userdetail, int id)
        {
            _userDetailService.UpdateByID(id, userdetail);
            return Ok(userdetail);
        }
    }
}
