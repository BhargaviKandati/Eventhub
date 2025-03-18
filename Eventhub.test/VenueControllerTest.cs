using Eventhub.Controllers;
using Eventhub.DTOs;
using Eventhub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventhub.Tests.Controllers
{
    [TestFixture] // This attribute is used to identify a class that contains NUnit tests
    public class VenueControllerTests
    {
        private EFCoreDBContext _context;
        private Mock<ILogger<VenueController>> _mockLogger;
        private VenueController _controller;

        [SetUp] 
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<EFCoreDBContext>()
                .UseInMemoryDatabase(databaseName: "EventhubTest")
                .Options;

            _context = new EFCoreDBContext(options);
            _mockLogger = new Mock<ILogger<VenueController>>();
            _controller = new VenueController(_context,_mockLogger.Object);

            ClearDatabase();
            SeedDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void ClearDatabase()
        {
            _context.Venues.RemoveRange(_context.Venues);
            _context.SaveChanges();
        }

        private void SeedDatabase()
        {
            var venue = new Venue
            {
                VenueId = 1,
                Name = "Test Venue",
                Location = "Test Location",
                Capacity = 1000
            };

            _context.Venues.Add(venue);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetVenues_ReturnsOkResult_WithListOfVenues()
        {
            // Act
            var result = await _controller.GetVenues();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<List<VenueDto>>());
            var venueList = okResult.Value as List<VenueDto>;
            Assert.That(venueList.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetVenue_ReturnsOkResult_WithVenue()
        {
            // Arrange
            var venueId = 1;

            // Act
            var result = await _controller.GetVenue(venueId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<VenueDto>());
        }

        [Test]
        public async Task GetVenue_ReturnsNotFound_WhenVenueNotFound()
        {
            // Arrange
            var venueId = 99;

            // Act
            var result = await _controller.GetVenue(venueId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateVenue_ReturnsCreatedAtActionResult_WithVenue()
        {
            // Arrange
            var venueDto = new VenueDto
            {
                Name = "New Venue",
                Location = "New Location",
                Capacity = 500
            };

            // Act
            var result = await _controller.CreateVenue(venueDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.That(createdResult.Value, Is.InstanceOf<Venue>());
        }

        [Test]
        public async Task CreateVenue_ReturnsBadRequest_WhenDtoIsInvalid()
        {
            // Arrange
            var venueDto = new VenueDto
            {
                Name = "", // Invalid value
                Location = "New Location",
                Capacity = 500
            };

            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.CreateVenue(venueDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UpdateVenue_ReturnsNoContent_WhenVenueUpdated()
        {
            // Arrange
            var venueId = 1;
            var venueDto = new VenueDto
            {
                VenueId = venueId,
                Name = "Updated Venue",
                Location = "Updated Location",
                Capacity = 1500
            };

            // Act
            var result = await _controller.UpdateVenue(venueId, venueDto);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task UpdateVenue_ReturnsNotFound_WhenVenueNotFound()
        {
            // Arrange
            var venueId = 99;
            var venueDto = new VenueDto
            {
                VenueId = venueId,
                Name = "Updated Venue",
                Location = "Updated Location",
                Capacity = 1500
            };

            // Act
            var result = await _controller.UpdateVenue(venueId, venueDto);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteVenue_ReturnsNoContent_WhenVenueDeleted()
        {
            // Arrange
            var venueId = 1;

            // Act
            var result = await _controller.DeleteVenue(venueId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteVenue_ReturnsNotFound_WhenVenueNotFound()
        {
            // Arrange
            var venueId = 99;

            // Act
            var result = await _controller.DeleteVenue(venueId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}