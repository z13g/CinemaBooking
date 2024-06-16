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
    public class GenericCinemaRepository_Test
    {
        private readonly DbContextOptions<Dbcontext> options;
        private readonly Dbcontext context;
        private readonly IPropertyValidationService validationService;

        public GenericCinemaRepository_Test()
        {
            options = new DbContextOptionsBuilder<Dbcontext>()
                .UseInMemoryDatabase(databaseName: "TestGenericCinemaDB")
                .Options;

            context = new Dbcontext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            validationService = new PropertyValidationService();

            // Populate initial data
            var Cinemas = new List<Cinema>
            {
                new Cinema { CinemaID = 100, Name = "Test1", Location = "City", NumberOfHalls = 12 },
                new Cinema { CinemaID = 101, Name = "Test2", Location = "City1", NumberOfHalls = 6 },
            };

            context.Cinemas.AddRange(Cinemas);
            context.SaveChanges();
        }

        [Fact]
        public void Create_AddsNewCinema()
        {
            // Arrange
            var repository = new GenericRepository<Cinema>(context, validationService);
            var newCinema = new Cinema { CinemaID = 102, Name = "Test3", Location = "City2", NumberOfHalls = 14 };

            // Act
            var result = repository.Create(newCinema);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test3", result.Name);
            Assert.True(context.Cinemas.Any(a => a.Name == "Test3" && a.NumberOfHalls == 14));
        }

        [Fact]
        public void Create_ThrowsException_WithInvalidProperties()
        {
            // Arrange
            var repository = new GenericRepository<Cinema>(context, validationService);
            var invalidCinema = new Cinema { Name = "Midtown" }; // Missing RegionID

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.Create(invalidCinema));
            Assert.Equal("Invalid properties", exception.Message);
        }

        [Fact]
        public void GetById_ReturnsCorrectArea()
        {
            // Arrange
            var repository = new GenericRepository<Cinema>(context, validationService);

            // Act
            var result = repository.GetById(100);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test1", result.Name);
        }

        [Fact]
        public void GetById_WhenNoCinemaFound_ReturnsNull()
        {
            // Arrange
            var repository = new GenericRepository<Cinema>(context, validationService);

            // Act
            var result = repository.GetById(3000);

            // Assert
            Assert.Null(result); // Ensures method handles "not found" by returning null
        }

        [Fact]
        public void GetAll_ReturnsAllCinema()
        {
            // Arrange
            var repository = new GenericRepository<Cinema>(context, validationService);

            // Act
            var results = repository.GetAllAsync();

            // Assert
            Assert.NotNull(results);
        }

        [Fact]
        public async Task GetAll_WhenNoCinema_ReturnsEmptyList()
        {
            // Arrange
            var repository = new GenericRepository<Cinema>(context, validationService);
            context.Cinemas.RemoveRange(context.Cinemas.ToList());
            context.SaveChanges();

            // Act
            var results = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }

        [Fact]
        public void UpdateByID_UpdatesCinema()
        {
            // Arrange
            var repository = new GenericRepository<Cinema>(context, validationService);
            var originalCinema = new Cinema { CinemaID = 2000, Name = "Test4", Location = "City4", NumberOfHalls = 10};
            context.Cinemas.Add(originalCinema);
            context.SaveChanges();

            // Act
            var cinemaToUpdate = repository.GetById(2000);  // Fetch the existing entity
            cinemaToUpdate.Name = "Test10";
            cinemaToUpdate.Location = "City4";
            cinemaToUpdate.NumberOfHalls = 11;
            repository.Update(cinemaToUpdate);  // Update the fetched entity
            var result = repository.GetById(2000);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test10", result.Name);
            Assert.Equal(11, result.NumberOfHalls);
        }

        [Fact]
        public void UpdateByID_WithInvalidProperties_ThrowsException()
        {
            // Arrange
            var repository = new GenericRepository<Cinema>(context, validationService);
            var originalCinema = new Cinema { CinemaID = 2000, Name = "Test4", Location = "City4", NumberOfHalls = 10 };
            context.Cinemas.Add(originalCinema);
            context.SaveChanges();

            var cinemaToUpdate = repository.GetById(2000);  // Fetch the existing entity
            cinemaToUpdate.Name = "Test10";
            cinemaToUpdate.Location = "";  // Invalid property
            cinemaToUpdate.NumberOfHalls = 11;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.Update(cinemaToUpdate));
            Assert.Equal("Invalid properties", exception.Message);
        }


        [Fact]
        public void DeleteByID_RemovesCinema()
        {
            // Arrange
            var repository = new GenericRepository<Cinema>(context, validationService);

            // Act
            var result = repository.DeleteById(100);

            // Assert
            Assert.True(result);
            Assert.False(context.Cinemas.Any(a => a.CinemaID == 100));
        }

        [Fact]
        public void DeleteByID_Throw_Exception()
        {
            // Arrange
            var repository = new GenericRepository<Cinema>(context, validationService);

            // Act
            var result = repository.DeleteById(3000);

            // Assert
            Assert.False(result);
        }
    }
}
