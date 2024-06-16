using H3CinemaBooking.Repository.Models.DTO;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;

namespace H3CinemaBooking.Repository.Models
{
    public class BookShow
    {
        public string? HallName { get; set; }
        public string? CinemaName { get; set; }

        public Movie? Movie { get; set; }

        public DateTime ShowDateTime { get; set; }

        public List<SeatDTO>? Seats { get; set; } = new List<SeatDTO>();
        public double? Price { get; set; }
        public int? showId { get; set; }

    }


}
