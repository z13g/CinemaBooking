using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Models.DTO;
using H3CinemaBooking.Repository.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Interfaces
{
    public interface IUserDetailService
    {
        List<UserDetailDTO> GetAllUserDetail();
        UserDetailDTO GetUserDetailById(int id);
        (string Hash, string Salt) CreateUserDetail(UserDetail userdetail);
        bool DeleteUserdetail(int id);
        public void UpdateByID(int Id, UserDetailDTO updatedUser);

        string RegisterUser(RegisterUserDTO registerUserDetail);
        UserDetail RegisterAdminWithoutToken(RegisterUserDTO registerUserDetail);
        public UserDetail RegisterCostumerWithoutToken(RegisterUserDTO registerUserDetail);
        bool CheckIfUserExistsFromEmail(string email);
        bool CheckIfUserExistsFromNumber(string phoneNumber);

        string Login(LoginUserDTO loginUserDetail);
        List<string> ValidateUserInput<T>(T userObjekt);
    }
}
