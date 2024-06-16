using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models.DTO_s
{
    public class UserDetailDTO
    {
        [Key]
        public int UserDetailID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleID { get; set; }
        public List<Booking>? Bookings { get; set; }
        public bool IsActive { get; set; }
    }
}
