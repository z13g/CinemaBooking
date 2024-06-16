using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models.DTO_s
{
    public class ShowDetailsDTO
    {
        public int ShowId { get; set; }
        public int CinemaID { get; set; }
        public string CinemaName { get; set; }
        public string HallName { get; set; }
        public DateTime ShowDateTime { get; set; }
        public string MovieTitle { get; set; }
    }
}
