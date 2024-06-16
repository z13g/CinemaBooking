using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models.DTO
{
    public class ReserveSeatDTO
    {
        public ReserveSeatDTO() { }
        public int ShowID {  get; set; }

        public int UserID { get; set; }

        public List<SeatReservationStatusDTO>? SeatList { get; set;}

    }
}
