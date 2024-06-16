using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models
{
    public class BookingDTO
    {
        [Key]
        public int BookingID { get; set; }
        public int ShowID { get; set; }
        public int NumberOfSeats { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public virtual List<BookingSeat> ?BookingSeats { get; set; }
        public Show ?Show { get; set; }
        public Movie ?Movie { get; set; }
        public Cinema ?Cinema { get; set; }

    }
}
