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
    public class GenericRegionRepository_Test
    {
        private readonly DbContextOptions<Dbcontext> options;
        private readonly Dbcontext context;
        private readonly IPropertyValidationService validationService;

        public GenericRegionRepository_Test()
        {
            options = new DbContextOptionsBuilder<Dbcontext>()
                .UseInMemoryDatabase(databaseName: "TestGenericRegionDB")
                .Options;

            context = new Dbcontext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            validationService = new PropertyValidationService();

            // Populate initial data
            var Regions = new List<Region>
            {
                new Region { RegionID = 101, RegionName = "Test1"},
                new Region { RegionID = 102, RegionName = "Test2"},
            };

            context.Regions.AddRange(Regions);
            context.SaveChanges();
        }

        [Fact]
        public void Create_AddsNewRegion()
        {
            // Arrange
            var repository = new GenericRepository<Region>(context, validationService);
            var newRegion = new Region { RegionID = 103, RegionName = "Test3" };

            // Act
            var result = repository.Create(newRegion);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test3", result.RegionName);
            Assert.True(context.Regions.Any(a => a.RegionName == "Test3" && a.RegionID == 103));
        }

        [Fact]
        public void Create_ThrowsException_WithInvalidProperties()
        {
            // Arrange
            var repository = new GenericRepository<Region>(context, validationService);
            var invalidRegion = new Region { RegionID = 103, RegionName = "" };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.Create(invalidRegion));
            Assert.Equal("Invalid properties", exception.Message);
        }

        [Fact]
        public void GetById_ReturnsCorrectRegion()
        {
            // Arrange
            var repository = new GenericRepository<Region>(context, validationService);

            // Act
            var result = repository.GetById(101);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test1", result.RegionName);
        }

        [Fact]
        public void GetById_WhenNoRegionFound_ReturnsNull()
        {
            // Arrange
            var repository = new GenericRepository<Region>(context, validationService);

            // Act
            var result = repository.GetById(3000);

            // Assert
            Assert.Null(result); // Ensures method handles "not found" by returning null
        }

        [Fact]
        public void GetAll_ReturnsAllRegion()
        {
            // Arrange
            var repository = new GenericRepository<Region>(context, validationService);

            // Act
            var results = repository.GetAllAsync();

            // Assert
            Assert.NotNull(results);
        }

        [Fact]
        public async Task GetAll_WhenNoRegion_ReturnsEmptyList()
        {
            // Arrange
            var repository = new GenericRepository<Region>(context, validationService);
            context.Regions.RemoveRange(context.Regions.ToList());
            context.SaveChanges();

            // Act
            var results = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }

        [Fact]
        public void UpdateByID_UpdatesRegion()
        {
            // Arrange
            var repository = new GenericRepository<Region>(context, validationService);
            var originalRegion = new Region { RegionID = 104, RegionName = "Test4"};
            context.Regions.Add(originalRegion);
            context.SaveChanges();

            // Act
            var regionToUpdate = repository.GetById(104);  // Fetch the existing entity
            regionToUpdate.RegionName = "Test10";
            repository.Update(regionToUpdate);  // Update the fetched entity
            var result = repository.GetById(104);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test10", result.RegionName);
        }

        [Fact]
        public void UpdateByID_WithInvalidProperties_ThrowsException()
        {
            // Arrange
            var repository = new GenericRepository<Region>(context, validationService);
            var originalRegion = new Region { RegionID = 104, RegionName = "Test4" };
            context.Regions.Add(originalRegion);
            context.SaveChanges();

            var cinemaHallToUpdate = repository.GetById(104);  // Fetch the existing entity
            cinemaHallToUpdate.RegionName = "";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.Update(cinemaHallToUpdate));
            Assert.Equal("Invalid properties", exception.Message);
        }


        [Fact]
        public void DeleteByID_RemovesRegionl()
        {
            // Arrange
            var repository = new GenericRepository<Region>(context, validationService);

            // Act
            var result = repository.DeleteById(101);

            // Assert
            Assert.True(result);
            Assert.False(context.Regions.Any(a => a.RegionID == 101));
        }

        [Fact]
        public void DeleteByID_Throw_Exception()
        {
            // Arrange
            var repository = new GenericRepository<Region>(context, validationService);

            // Act
            var result = repository.DeleteById(3000);

            // Assert
            Assert.False(result);
        }
    }
}
