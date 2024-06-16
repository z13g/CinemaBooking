using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models.DTO
{
    public class SeatDTO
    {
        public int HallID { get; set; }
        public int SeatID { get; set; }
        public int SeatNumber { get; set; }
        public char SeatRow { get; set; }
        // Andre properties...
        public string SeatStatus { get; set; }
    }
}
