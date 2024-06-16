using H3CinemaBooking.Repository.Models;
using System;
using System.Collections.Generic;

namespace H3CinemaBooking.Repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Create(TEntity entity);
        TEntity GetById(int id);
        public Task<List<TEntity>> GetAllAsync();
        public void Update(TEntity entity);
        bool DeleteById(int id);
    }
}
