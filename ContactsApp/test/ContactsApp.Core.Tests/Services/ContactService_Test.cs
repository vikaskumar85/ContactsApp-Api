using ContactsApp.Core.Models;
using ContactsApp.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Assert = Xunit.Assert;

namespace ContactsApp.Core.Tests.Services
{
    public class ContactService_Tests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IHostingEnvironment> _mockHostingEnvironment;
        private readonly ContactService _contactService;

        public ContactService_Tests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockHostingEnvironment = new Mock<IHostingEnvironment>();
            _contactService = new ContactService(_mockConfiguration.Object, _mockHostingEnvironment.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllContacts()
        {
            // Arrange
            var contactList = new List<ContactModel>
            {
                new ContactModel { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
            };

            _mockHostingEnvironment.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _mockConfiguration.Setup(cfg => cfg["DataFileName"]).Returns("contacts.json");

            var json = JsonConvert.SerializeObject(contactList);
            File.WriteAllText("contacts.json", json);

            // Act
            var result = await _contactService.GetAll();

            // Assert
            Assert.Equal(1, result.TotalNoOfContacts);
            Assert.Single(result.Contacts);
            Assert.Equal("John", result.Contacts.First().FirstName);
        }

        [Fact]
        public async Task Insert_AddsNewContact()
        {
            // Arrange
            var initialContacts = new List<ContactModel>
            {
                new ContactModel { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
            };

            var newContact = new ContactModel { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" };

            _mockHostingEnvironment.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _mockConfiguration.Setup(cfg => cfg["DataFileName"]).Returns("contacts.json");

            File.WriteAllText("contacts.json", JsonConvert.SerializeObject(initialContacts));

            // Act
            var result = await _contactService.Insert(newContact);

            // Assert
            Assert.True(result);

            var updatedContacts = JsonConvert.DeserializeObject<List<ContactModel>>(File.ReadAllText("contacts.json"));
            Assert.Equal(2, updatedContacts.Count);
            Assert.Equal("Jane", updatedContacts.Last().FirstName);
        }

        [Fact]
        public async Task Update_UpdatesExistingContact()
        {
            // Arrange
            var contacts = new List<ContactModel>
            {
                new ContactModel { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
            };

            var updatedContact = new ContactModel { Id = 1, FirstName = "Johnny", LastName = "Doe", Email = "johnny.doe@example.com" };

            _mockHostingEnvironment.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _mockConfiguration.Setup(cfg => cfg["DataFileName"]).Returns("contacts.json");

            File.WriteAllText("contacts.json", JsonConvert.SerializeObject(contacts));

            // Act
            var result = await _contactService.Update(updatedContact);

            // Assert
            Assert.True(result);

            var updatedContacts = JsonConvert.DeserializeObject<List<ContactModel>>(File.ReadAllText("contacts.json"));
            var contact = updatedContacts.FirstOrDefault(c => c.Id == 1);
            Assert.NotNull(contact);
            Assert.Equal("Johnny", contact.FirstName);
        }

        [Fact]
        public async Task Delete_RemovesContact()
        {
            // Arrange
            var contacts = new List<ContactModel>
            {
                new ContactModel { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new ContactModel { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
            };

            _mockHostingEnvironment.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _mockConfiguration.Setup(cfg => cfg["DataFileName"]).Returns("contacts.json");

            File.WriteAllText("contacts.json", JsonConvert.SerializeObject(contacts));

            // Act
            var result = await _contactService.Delete(1);

            // Assert
            Assert.True(result);

            var remainingContacts = JsonConvert.DeserializeObject<List<ContactModel>>(File.ReadAllText("contacts.json"));
            Assert.Single(remainingContacts);
            Assert.DoesNotContain(remainingContacts, c => c.Id == 1);
        }


    }

}