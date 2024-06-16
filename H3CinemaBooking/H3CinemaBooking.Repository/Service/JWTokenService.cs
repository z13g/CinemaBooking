using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Service
{
    public class JWTokenService : IJWTokenService
    {
        private readonly IUserDetailRepository _userDetailRepository;
        private readonly IConfiguration _configuration;
        private readonly string _jwtSecret;

        public JWTokenService(IUserDetailRepository userDetailRepository, IConfiguration configuration)
        {
            _userDetailRepository = userDetailRepository;
            _configuration = configuration;
            _jwtSecret = _configuration.GetSection("AppSettings:Token").Value!;
        }

        public string CreateToken(UserDetail userDetail)
        {
            var roleInfo = _userDetailRepository.GetRole(userDetail.RoleID);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, userDetail.UserDetailID.ToString()),
                new Claim(ClaimTypes.Email, userDetail.Email),
                new Claim(ClaimTypes.Name, userDetail.Name),
                new Claim(ClaimTypes.MobilePhone, userDetail.PhoneNumber)

            };

            if (roleInfo != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleInfo.RoleName));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Customer"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (SecurityTokenException)
            {
                // Token is not valid
                return null;
            }
            catch (Exception)
            {
                // Other exceptions
                return null;
            }
        }

        public int? GetUserIDFromToken(ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
}
