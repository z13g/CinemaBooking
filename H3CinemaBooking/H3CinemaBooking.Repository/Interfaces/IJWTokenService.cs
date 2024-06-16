using H3CinemaBooking.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Interfaces
{
    public interface IJWTokenService
    {
        string CreateToken(UserDetail userDetail);
        ClaimsPrincipal ValidateToken(string token);
        int? GetUserIDFromToken(ClaimsPrincipal user);
    }
}
