using H3CinemaBooking.Repository.Data;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace H3CinemaBooking.Repository.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly Dbcontext context;
        private readonly DbSet<TEntity> dbSet;
        private readonly IPropertyValidationService validationService;

        public GenericRepository(Dbcontext _context, IPropertyValidationService _validationService)
        {
            context = _context;
            dbSet = context.Set<TEntity>();
            validationService = _validationService;
        }

        public TEntity Create(TEntity entity)
        {
            if (!validationService.ValidateProperties(entity, new string[] { "AreaID", "Cinemas", "CinemaID", "Area", "Seats", "HallsID", "RegionID", "Areas", "RoleID" }))
            {
                throw new ArgumentException("Invalid properties");
            }

            dbSet.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public TEntity GetById(int id)
        {
            return dbSet.Find(id);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
             return await dbSet.ToListAsync();
        }

        public interface IEntity
        {
            int Id { get; set; }
        }

        public void Update(TEntity entity)
        {
            if (!validationService.ValidateProperties(entity, new string[] { "AreaID", "Cinemas", "Area", "Seats", "HallsID", "RegionID", "Areas", "RoleID" }))
            {
                throw new ArgumentException("Invalid properties");
            }

            if (entity != null)
            {
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }



        public bool DeleteById(int id)
        {
            var entity = dbSet.Find(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
