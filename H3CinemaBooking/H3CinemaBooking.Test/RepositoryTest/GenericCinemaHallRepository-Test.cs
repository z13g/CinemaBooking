using H3CinemaBooking.Repository.Data;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Repositories;
using H3CinemaBooking.Repository.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace H3CinemaBooking.Test.RepositoryTest
{
    public class GenericCinemaHallRepository_Test
    {
        private readonly DbContextOptions<Dbcontext> options;
        private readonly Dbcontext context;
        private readonly IPropertyValidationService validationService;

        public GenericCinemaHallRepository_Test()
        {
            options = new DbContextOptionsBuilder<Dbcontext>()
                .UseInMemoryDatabase(databaseName: "TestGenericCinemaHallDB")
                .Options;

            context = new Dbcontext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            validationService = new PropertyValidationService();

            // Populate initial data
            var Cinemahalls = new List<CinemaHall>
            {
                new CinemaHall { HallsID = 101, HallName = "Test1", CinemaID = 1 },
                new CinemaHall {HallsID = 102, HallName = "Test2", CinemaID = 1},
            };

            context.CinemaHalls.AddRange(Cinemahalls);
            context.SaveChanges();
        }

        [Fact]
        public void Create_AddsNewCinemaHall()
        {
            // Arrange
            var repository = new GenericRepository<CinemaHall>(context, validationService);
            var newcinemaHall = new CinemaHall {HallsID = 103, HallName = "Test3", CinemaID = 1};

            // Act
            var result = repository.Create(newcinemaHall);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test3", result.HallName);
            Assert.True(context.CinemaHalls.Any(a => a.HallName == "Test3" && a.CinemaID == 1));
        }

        [Fact]
        public void Create_ThrowsException_WithInvalidProperties()
        {
            // Arrange
            var repository = new GenericRepository<CinemaHall>(context, validationService);
            var invalidCInemaHall = new CinemaHall { HallsID = 103, HallName = "", CinemaID = 1 };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.Create(invalidCInemaHall));
            Assert.Equal("Invalid properties", exception.Message);
        }

        [Fact]
        public void GetById_ReturnsCorrectArea()
        {
            // Arrange
            var repository = new GenericRepository<CinemaHall>(context, validationService);

            // Act
            var result = repository.GetById(101);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test1", result.HallName);
        }

        [Fact]
        public void GetById_WhenNoCinemaHallFound_ReturnsNull()
        {
            // Arrange
            var repository = new GenericRepository<CinemaHall>(context, validationService);

            // Act
            var result = repository.GetById(3000);

            // Assert
            Assert.Null(result); // Ensures method handles "not found" by returning null
        }

        [Fact]
        public void GetAll_ReturnsAllCinemaHall()
        {
            // Arrange
            var repository = new GenericRepository<CinemaHall>(context, validationService);

            // Act
            var results = repository.GetAllAsync();

            // Assert
            Assert.NotNull(results);
        }

        [Fact]
        public async Task GetAll_WhenNoCinemaHall_ReturnsEmptyList()
        {
            // Arrange
            var repository = new GenericRepository<CinemaHall>(context, validationService);
            context.CinemaHalls.RemoveRange(context.CinemaHalls.ToList());
            context.SaveChanges();

            // Act
            var results = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }

        [Fact]
        public void UpdateByID_UpdatesCinemaHall()
        {
            // Arrange
            var repository = new GenericRepository<CinemaHall>(context, validationService);
            var originalCinemaHall = new CinemaHall { HallsID = 104, HallName = "Test4", CinemaID = 1 }; 
            context.CinemaHalls.Add(originalCinemaHall);
            context.SaveChanges();

            // Act
            var cinemaHallToUpdate = repository.GetById(104);  // Fetch the existing entity
            cinemaHallToUpdate.HallName = "Test10";
            cinemaHallToUpdate.CinemaID = 2;
            repository.Update(cinemaHallToUpdate);  // Update the fetched entity
            var result = repository.GetById(104);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test10", result.HallName);
            Assert.Equal(2, result.CinemaID);
        }

        [Fact]
        public void UpdateByID_WithInvalidProperties_ThrowsException()
        {
            // Arrange
            var repository = new GenericRepository<CinemaHall>(context, validationService);
            var originalCinemaHall = new CinemaHall { HallsID = 104, HallName = "Test4", CinemaID = 1 };
            context.CinemaHalls.Add(originalCinemaHall);
            context.SaveChanges();

            var cinemaHallToUpdate = repository.GetById(104);  // Fetch the existing entity
            cinemaHallToUpdate.HallName = "";
            cinemaHallToUpdate.CinemaID = 2;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.Update(cinemaHallToUpdate));
            Assert.Equal("Invalid properties", exception.Message);
        }


        [Fact]
        public void DeleteByID_RemovesCinemaHall()
        {
            // Arrange
            var repository = new GenericRepository<CinemaHall>(context, validationService);

            // Act
            var result = repository.DeleteById(101);

            // Assert
            Assert.True(result);
            Assert.False(context.CinemaHalls.Any(a => a.HallsID == 101));
        }

        [Fact]
        public void DeleteByID_Throw_Exception()
        {
            // Arrange
            var repository = new GenericRepository<CinemaHall>(context, validationService);

            // Act
            var result = repository.DeleteById(3000);

            // Assert
            Assert.False(result);
        }
    }
}
