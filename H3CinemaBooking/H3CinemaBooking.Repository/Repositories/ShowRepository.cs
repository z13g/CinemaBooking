using H3CinemaBooking.Repository.Data;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Models.DTO_s;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly Dbcontext context;
        private readonly IPropertyValidationService validationService;

        public ShowRepository(Dbcontext _context, IPropertyValidationService _validationService)
        {
            context = _context;
            validationService = _validationService;
        }

        public Show Create(Show show)
        {
            if (!validationService.ValidateProperties(show, new string[] { "ShowID" }))
            {
                throw new ArgumentException("Invalid show properties.");
            }

            context.Shows.Add(show);
            context.SaveChanges();
            return show;
        }
        public Show GetById(int Id)
        {
            var result = context.Shows.FirstOrDefault(c => c.ShowID == Id);
            return result;
        }

        //TODO: Get All Costumer
        public List<Show> GetAll()
        {
            var result = context.Shows.ToList();

            return result;
        }

        public void UpdateByID(int Id, Show updatedShow)
        {
            if (!validationService.ValidateProperties(updatedShow, new string[] { "ShowID" }))
            {
                throw new ArgumentException("Invalid show properties.");
            }

            var show = context.Shows.FirstOrDefault(s => s.ShowID == Id);

            if (show != null)
            {
                show.HallID = updatedShow.HallID;
                show.MovieID = updatedShow.MovieID;
                show.Price = updatedShow.Price;
                show.ShowDateTime = updatedShow.ShowDateTime;

                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException($"No show found with ID {Id}.");
            }
        }

        public bool DeleteByID(int Id)
        {
            var show = context.Shows.FirstOrDefault(c => c.ShowID == Id);
            if (show != null)
            {
                context.Remove(show);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public double GetShowPrice(int showId)
        {
            var show = context.Shows.FirstOrDefault(s => s.ShowID == showId);
            if (show != null)
            {
                return show.Price;
            }
            throw new InvalidOperationException($"Show with ID {showId} not found.");
        }


    }
}
