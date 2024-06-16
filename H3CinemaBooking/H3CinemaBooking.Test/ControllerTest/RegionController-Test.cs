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
    public class RegionController_Test
    {
        private readonly Mock<IGenericRepository<Region>> _mockRepo;
        private readonly RegionController _controller;

        public RegionController_Test()
        {
            _mockRepo = new Mock<IGenericRepository<Region>>();
            _controller = new RegionController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenRegionsExist()
        {
            // Arrange
            var regions = new List<Region>
            {
                new Region { RegionID = 1, RegionName = "TestRegion1" },
                new Region { RegionID = 2, RegionName = "TestRegion2" }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(regions);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Region>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAll_ReturnsNoContent_WhenNoRegionsExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Region>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsOk_WhenRegionExists()
        {
            // Arrange
            var region = new Region { RegionID = 1, RegionName = "TestRegion" };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(region);

            // Act
            var result = _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Region>(okResult.Value);
            Assert.Equal(1, returnValue.RegionID);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenRegionDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Region)null);

            // Act
            var result = _controller.GetById(100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsOk_WhenRegionIsCreated()
        {
            // Arrange
            var region = new Region { RegionID = 1, RegionName = "TestRegion" };
            _mockRepo.Setup(repo => repo.Create(region));

            // Act
            var result = _controller.Post(region);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Region>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<Region>(okResult.Value);
            Assert.Equal(1, returnValue.RegionID);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenRegionIsNull()
        {
            // Act
            var result = _controller.Post(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Region>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Region data is required", badRequestResult.Value);
        }

        [Fact]
        public void Update_ReturnsOk_WhenRegionIsUpdated()
        {
            // Arrange
            var region = new Region { RegionID = 1, RegionName = "UpdatedRegion" };
            var existingRegion = new Region { RegionID = 1, RegionName = "TestRegion" };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(existingRegion);

            // Act
            var result = _controller.Update(1, region);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Region>(okResult.Value);
            Assert.Equal("UpdatedRegion", returnValue.RegionName);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenRegionDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Region)null);

            // Act
            var result = _controller.Update(1, new Region { RegionID = 1 });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsBadRequest_WhenRegionIsNull()
        {
            // Act
            var result = _controller.Update(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenRegionIsDeleted()
        {
            // Arrange
            var region = new Region { RegionID = 1, RegionName = "TestRegion" };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(region);
            _mockRepo.Setup(repo => repo.DeleteById(1));

            // Act
            var result = _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null);
            Assert.Equal("Region deleted successfully", returnValue);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenRegionDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Region)null);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
