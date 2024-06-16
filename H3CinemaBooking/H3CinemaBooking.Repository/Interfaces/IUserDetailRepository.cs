using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Interfaces
{
    public interface IUserDetailRepository
    {
        UserDetail Create(UserDetail userDetail);
        UserDetail GetById(int id);
        List<UserDetail> GetAll();
        void DeleteByID(int id);
        void UpdateByID(int Id, UserDetailDTO updatedUser);

        UserDetail GetByEmail(string email);
        UserDetail GetByPhoneNumber(string phoneNumber);

        Roles GetRole(int roleId);
    }
}
