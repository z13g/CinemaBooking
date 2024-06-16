using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models
{
    public class Roles
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
