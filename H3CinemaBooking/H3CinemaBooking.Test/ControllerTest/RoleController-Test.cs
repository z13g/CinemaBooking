using H3CinemaBooking.API.Controllers;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace H3CinemaBooking.Test.ControllerTest
{
    public class RoleController_Test
    {
        private readonly Mock<IGenericRepository<Roles>> _mockRepo;
        private readonly RoleController _controller;

        public RoleController_Test()
        {
            _mockRepo = new Mock<IGenericRepository<Roles>>();
            _controller = new RoleController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenRolesExist()
        {
            // Arrange
            var roles = new List<Roles>
            {
                new Roles { RoleID = 1, RoleName = "TestRole1" },
                new Roles { RoleID = 2, RoleName = "TestRole2" }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(roles);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Roles>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAll_ReturnsNoContent_WhenNoRolesExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Roles>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsOk_WhenRoleExists()
        {
            // Arrange
            var role = new Roles { RoleID = 1, RoleName = "TestRole" };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(role);

            // Act
            var result = _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Roles>(okResult.Value);
            Assert.Equal(1, returnValue.RoleID);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenRoleDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Roles)null);

            // Act
            var result = _controller.GetById(100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsOk_WhenRoleIsCreated()
        {
            // Arrange
            var role = new Roles { RoleID = 1, RoleName = "TestRole" };
            _mockRepo.Setup(repo => repo.Create(role));

            // Act
            var result = _controller.Post(role);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Roles>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<Roles>(okResult.Value);
            Assert.Equal(1, returnValue.RoleID);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenRoleIsNull()
        {
            // Act
            var result = _controller.Post(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Roles>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Role data is required", badRequestResult.Value);
        }

        [Fact]
        public void Update_ReturnsOk_WhenRoleIsUpdated()
        {
            // Arrange
            var role = new Roles { RoleID = 1, RoleName = "UpdatedRole" };
            var existingRole = new Roles { RoleID = 1, RoleName = "TestRole" };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(existingRole);

            // Act
            var result = _controller.Update(1, role);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Roles>(okResult.Value);
            Assert.Equal("UpdatedRole", returnValue.RoleName);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenRoleDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Roles)null);

            // Act
            var result = _controller.Update(1, new Roles { RoleID = 1 });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsBadRequest_WhenRoleIsNull()
        {
            // Act
            var result = _controller.Update(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenRoleIsDeleted()
        {
            // Arrange
            var role = new Roles { RoleID = 1, RoleName = "TestRole" };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(role);
            _mockRepo.Setup(repo => repo.DeleteById(1));

            // Act
            var result = _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null);
            Assert.Equal("Role deleted successfully", returnValue);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenRoleDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Roles)null);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
