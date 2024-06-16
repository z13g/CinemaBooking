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
    public class GenericRoleRepository_Test
    {
        private readonly DbContextOptions<Dbcontext> options;
        private readonly Dbcontext context;
        private readonly IPropertyValidationService validationService;

        public GenericRoleRepository_Test()
        {
            options = new DbContextOptionsBuilder<Dbcontext>()
                .UseInMemoryDatabase(databaseName: "TestGenericRoleDB")
                .Options;

            context = new Dbcontext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            validationService = new PropertyValidationService();

            // Populate initial data
            var Roles = new List<Roles>
            {
                new Roles { RoleID = 100, RoleName = "Test1"},
                new Roles { RoleID = 101, RoleName = "Test2" },
            };

            context.Roles.AddRange(Roles);
            context.SaveChanges();
        }

        [Fact]
        public void Create_AddsNewRole()
        {
            // Arrange
            var repository = new GenericRepository<Roles>(context, validationService);
            var newRoles = new Roles {RoleID = 102, RoleName = "Test3" };

            // Act
            var result = repository.Create(newRoles);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test3", result.RoleName);
            Assert.True(context.Roles.Any(a => a.RoleName == "Test3" && a.RoleID == 102));
        }

        [Fact]
        public void Create_ThrowsException_WithInvalidProperties()
        {
            // Arrange
            var repository = new GenericRepository<Roles>(context, validationService);
            var invalidRoles = new Roles { RoleName = "" }; // Missing RegionID

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.Create(invalidRoles));
            Assert.Equal("Invalid properties", exception.Message);
        }

        [Fact]
        public void GetById_ReturnsCorrectRole()
        {
            // Arrange
            var repository = new GenericRepository<Roles>(context, validationService);

            // Act
            var result = repository.GetById(100);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test1", result.RoleName);
        }

        [Fact]
        public void GetById_WhenNoRoleFound_ReturnsNull()
        {
            // Arrange
            var repository = new GenericRepository<Roles>(context, validationService);

            // Act
            var result = repository.GetById(3000);

            // Assert
            Assert.Null(result); // Ensures method handles "not found" by returning null
        }

        [Fact]
        public void GetAll_ReturnsAllRole()
        {
            // Arrange
            var repository = new GenericRepository<Roles>(context, validationService);

            // Act
            var results = repository.GetAllAsync();

            // Assert
            Assert.NotNull(results);
        }

        [Fact]
        public async Task GetAll_WhenNoRole_ReturnsEmptyList()
        {
            // Arrange
            var repository = new GenericRepository<Roles>(context, validationService);
            context.Roles.RemoveRange(context.Roles.ToList());
            context.SaveChanges();

            // Act
            var results = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }

        [Fact]
        public void UpdateByID_UpdatesRole()
        {
            // Arrange
            var repository = new GenericRepository<Roles>(context, validationService);
            var originalRole = new Roles { RoleID = 2000, RoleName = "Test4"};
            context.Roles.Add(originalRole);
            context.SaveChanges();

            // Act
            var rolesToUpdate = repository.GetById(2000);  // Fetch the existing entity
            rolesToUpdate.RoleName = "Test10";
            repository.Update(rolesToUpdate);  // Update the fetched entity
            var result = repository.GetById(2000);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test10", result.RoleName);
            Assert.Equal(2000, result.RoleID);
        }

        [Fact]
        public void UpdateByID_WithInvalidProperties_ThrowsException()
        {
            // Arrange
            var repository = new GenericRepository<Roles>(context, validationService);
            var originalRole = new Roles { RoleID = 2000, RoleName = "Test4" };
            context.Roles.Add(originalRole);
            context.SaveChanges();

            var roleToUpdate = repository.GetById(2000);  // Fetch the existing entity
            roleToUpdate.RoleName = "";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.Update(roleToUpdate));
            Assert.Equal("Invalid properties", exception.Message);
        }


        [Fact]
        public void DeleteByID_RemovesRole()
        {
            // Arrange
            var repository = new GenericRepository<Roles>(context, validationService);

            // Act
            var result = repository.DeleteById(100);

            // Assert
            Assert.True(result);
            Assert.False(context.Roles.Any(a => a.RoleID == 100));
        }

        [Fact]
        public void DeleteByID_Throw_Exception()
        {
            // Arrange
            var repository = new GenericRepository<Roles>(context, validationService);

            // Act
            var result = repository.DeleteById(3000);

            // Assert
            Assert.False(result);
        }
    }
}
