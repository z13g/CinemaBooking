using H3CinemaBooking.API.Controllers;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Test.ControllerTest
{
    public class AreaController_Test
    {
        private readonly Mock<IGenericRepository<Area>> _mockRepo;
        private readonly AreaController _controller;

        public AreaController_Test()
        {
            _mockRepo = new Mock<IGenericRepository<Area>>();
            _controller = new AreaController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenAreasExist()
        {
            // Arrange
            var areas = new List<Area> { new Area { AreaID = 1, AreaName = "TestArea", RegionID = 1 } };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(areas);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Area>>(okResult.Value);
            Assert.Single(returnValue);
        }


        [Fact]
        public async Task GetAll_ReturnsNoContent_WhenNoAreasExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Area>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsOk_WhenAreaExists()
        {
            // Arrange
            var area = new Area { AreaID = 1, AreaName = "TestArea", RegionID = 1 };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(area);

            // Act
            var result = _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Area>(okResult.Value);
            Assert.Equal(1, returnValue.AreaID);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenAreaDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Area)null);

            // Act
            var result = _controller.GetById(100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsOk_WhenAreaIsCreated()
        {
            // Arrange
            var area = new Area { AreaID = 1, AreaName = "TestArea", RegionID = 1 };
            _mockRepo.Setup(repo => repo.Create(area));

            // Act
            var result = _controller.Post(area);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Area>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<Area>(okResult.Value);
            Assert.Equal(1, returnValue.AreaID);
        }



        [Fact]
        public void Post_ReturnsBadRequest_WhenAreaIsNull()
        {
            // Act
            var result = _controller.Post(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Area>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Area data is required", badRequestResult.Value);
        }

        [Fact]
        public void Update_ReturnsOk_WhenAreaIsUpdated()
        {
            // Arrange
            var area = new Area { AreaID = 1, AreaName = "UpdatedArea", RegionID = 2 };
            var existingArea = new Area { AreaID = 1, AreaName = "TestArea", RegionID = 1 };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(existingArea);

            // Act
            var result = _controller.Update(1, area);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Area>(okResult.Value);
            Assert.Equal("UpdatedArea", returnValue.AreaName);
        }


        [Fact]
        public void Update_ReturnsNotFound_WhenAreaDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Area)null);

            // Act
            var result = _controller.Update(1, new Area { AreaID = 1 });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsBadRequest_WhenAreaIsNull()
        {
            // Act
            var result = _controller.Update(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenAreaIsDeleted()
        {
            // Arrange
            var area = new Area { AreaID = 1, AreaName = "TestArea", RegionID = 1 };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(area);
            _mockRepo.Setup(repo => repo.DeleteById(1));

            // Act
            var result = _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null);
            Assert.Equal("Area deleted successfully", returnValue);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenAreaDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Area)null);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
