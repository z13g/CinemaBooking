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
    public class GenericAreaRepository_Test
    {
        private readonly DbContextOptions<Dbcontext> options;
        private readonly Dbcontext context;
        private readonly IPropertyValidationService validationService;

        public GenericAreaRepository_Test()
        {
            options = new DbContextOptionsBuilder<Dbcontext>()
                .UseInMemoryDatabase(databaseName: "TestGenericAreaDB")
                .Options;

            context = new Dbcontext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            validationService = new PropertyValidationService();

            // Populate initial data
            var areas = new List<Area>
            {
                new Area { AreaID = 100, AreaName = "Downtown", RegionID = 1 },
                new Area { AreaID = 101, AreaName = "Uptown", RegionID = 2 },
            };

            context.Areas.AddRange(areas);
            context.SaveChanges();
        }

        [Fact]
        public void Create_AddsNewArea()
        {
            // Arrange
            var repository = new GenericRepository<Area>(context, validationService);
            var newArea = new Area { AreaName = "Midtown", RegionID = 1 };

            // Act
            var result = repository.Create(newArea);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Midtown", result.AreaName);
            Assert.True(context.Areas.Any(a => a.AreaName == "Midtown" && a.RegionID == 1));
        }

        [Fact]
        public void Create_ThrowsException_WithInvalidProperties()
        {
            // Arrange
            var repository = new GenericRepository<Area>(context, validationService);
            var invalidArea = new Area { AreaName = "" }; // Missing RegionID

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.Create(invalidArea));
            Assert.Equal("Invalid properties", exception.Message);
        }

        [Fact]
        public void GetById_ReturnsCorrectArea()
        {
            // Arrange
            var repository = new GenericRepository<Area>(context, validationService);

            // Act
            var result = repository.GetById(100);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Downtown", result.AreaName);
        }

        [Fact]
        public void GetById_WhenNoAreaFound_ReturnsNull()
        {
            // Arrange
            var repository = new GenericRepository<Area>(context, validationService);

            // Act
            var result = repository.GetById(3000);

            // Assert
            Assert.Null(result); // Ensures method handles "not found" by returning null
        }

        [Fact]
        public void GetAll_ReturnsAllAreas()
        {
            // Arrange
            var repository = new GenericRepository<Area>(context, validationService);

            // Act
            var results = repository.GetAllAsync();

            // Assert
            Assert.NotNull(results);
        }

        [Fact]
        public async Task GetAll_WhenNoArea_ReturnsEmptyList()
        {
            // Arrange
            var repository = new GenericRepository<Area>(context, validationService);
            context.Areas.RemoveRange(context.Areas.ToList());
            context.SaveChanges();

            // Act
            var results = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }

        [Fact]
        public void UpdateByID_UpdatesArea()
        {
            // Arrange
            var repository = new GenericRepository<Area>(context, validationService);
            var originalArea = new Area { AreaID = 2000, AreaName = "Downtown", RegionID = 1 };
            context.Areas.Add(originalArea);
            context.SaveChanges();

            // Act
            var areaToUpdate = repository.GetById(2000);  // Fetch the existing entity
            areaToUpdate.AreaName = "New Downtown";
            areaToUpdate.RegionID = 2;
            repository.Update(areaToUpdate);  // Update the fetched entity
            var result = repository.GetById(2000);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Downtown", result.AreaName);
            Assert.Equal(2, result.RegionID);
        }


        [Fact]
        public void UpdateByID_WithInvalidProperties_ThrowsException()
        {
            // Arrange
            var repository = new GenericRepository<Area>(context, validationService);
            var originalArea = new Area { AreaID = 2005, AreaName = "Downtown", RegionID = 1 };
            context.Areas.Add(originalArea);
            context.SaveChanges();

            var areaToUpdate = repository.GetById(2005);  // Fetch the existing entity
            areaToUpdate.AreaName = "";  // Invalid property
            areaToUpdate.RegionID = 2;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.Update(areaToUpdate));
            Assert.Equal("Invalid properties", exception.Message);
        }

        [Fact]
        public void DeleteByID_RemovesArea()
        {
            // Arrange
            var repository = new GenericRepository<Area>(context, validationService);

            // Act
            var result = repository.DeleteById(100);

            // Assert
            Assert.True(result);
            Assert.False(context.Areas.Any(a => a.AreaID == 100));
        }

        [Fact]
        public void DeleteByID_Throw_Exception()
        {
            // Arrange
            var repository = new GenericRepository<Area>(context, validationService);

            // Act
            var result = repository.DeleteById(3000);

            // Assert
            Assert.False(result);
        }
    }
}  
