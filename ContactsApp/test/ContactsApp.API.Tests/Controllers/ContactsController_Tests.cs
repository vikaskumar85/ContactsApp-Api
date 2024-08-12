using ContactsApp.API.Controllers;
using ContactsApp.Core.Interfaces;
using ContactsApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace ContactsApp.API.Tests.Controllers
{
    public class ContactsController_Tests
    {
        private readonly Mock<IContactService> _mockContactService;
        private readonly Mock<ILogger<ContactsController>> _mockLogger;
        private readonly ContactsController _controller;

        public ContactsController_Tests()
        {
            _mockContactService = new Mock<IContactService>();
            _mockLogger = new Mock<ILogger<ContactsController>>();
            _controller = new ContactsController(_mockLogger.Object, _mockContactService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithContacts()
        {
            // Arrange
            var contacts = new ResultModel
            {
                TotalNoOfContacts = 1,
                Contacts = new List<ContactModel>
                {
                    new ContactModel { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
                }
            };

            _mockContactService.Setup(s => s.GetAll()).ReturnsAsync(contacts);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ResultModel>(okResult.Value);
            Assert.Equal(1, returnValue.TotalNoOfContacts);
        }

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenNoContacts()
        {
            // Arrange
            _mockContactService.Setup(s => s.GetAll()).ReturnsAsync((ResultModel)null);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Insert_ReturnsOkResult_WhenInsertSuccessful()
        {
            // Arrange
            var contact = new ContactModel { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" };
            _mockContactService.Setup(s => s.Insert(contact)).ReturnsAsync(true);

            // Act
            var result = await _controller.Insert(contact);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task Insert_ReturnsBadRequest_WhenModelIsNull()
        {
            // Act
            var result = await _controller.Insert(null);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenUpdateSuccessful()
        {
            // Arrange
            var contact = new ContactModel { Id = 1, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" };
            _mockContactService.Setup(s => s.Update(contact)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(contact);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelIsNull()
        {
            // Act
            var result = await _controller.Update(null);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenDeleteSuccessful()
        {
            // Arrange
            var id = 1;
            _mockContactService.Setup(s => s.Delete(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _controller.Delete(0);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }
    }
}
