using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models.DTO
{
    public class ReserveSeatResultDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<SeatReservationStatusDTO> SeatStatuses { get; set; } = new List<SeatReservationStatusDTO>();
    }

    public class SeatReservationStatusDTO
    {
        public int SeatID { get; set; }
        public string SeatStatus { get; set; }
    }
}
