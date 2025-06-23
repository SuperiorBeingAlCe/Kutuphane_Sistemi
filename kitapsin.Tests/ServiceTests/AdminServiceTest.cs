using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kitapsin.Server.Dto;
using kitapsin.Server.Services;
using Moq;

namespace kitapsin.Tests.ServiceTests
{
    public class AdminServiceTest
    {
        private readonly Mock<IAdminService> _adminServiceMock;

        public AdminServiceTest()
        {
            _adminServiceMock = new Mock<IAdminService>();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAdmins()
        {
            // Arrange
            var admins = new List<DtoAdminResponse>
                {
                    new DtoAdminResponse { Id = 1, Username = "admin1", Email = "admin1@test.com" },
                    new DtoAdminResponse { Id = 2, Username = "admin2", Email = "admin2@test.com" }
                };
            _adminServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(admins);

            // Act
            var result = await _adminServiceMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<DtoAdminResponse>)result).Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAdmin_WhenExists()
        {
            // Arrange
            var admin = new DtoAdminResponse { Id = 1, Username = "admin1", Email = "admin1@test.com" };
            _adminServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(admin);

            // Act
            var result = await _adminServiceMock.Object.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("admin1", result.Username);
        }

        [Fact]
        public async Task GetByUsernameAsync_ShouldReturnAdmin_WhenExists()
        {
            // Arrange
            var admin = new DtoAdminResponse { Id = 1, Username = "admin1", Email = "admin1@test.com" };
            _adminServiceMock.Setup(s => s.GetByUsernameAsync("admin1")).ReturnsAsync(admin);

            // Act
            var result = await _adminServiceMock.Object.GetByUsernameAsync("admin1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedAdmin()
        {
            // Arrange
            var dto = new DtoAdminCreate { Username = "admin3", Password = "pass", Email = "admin3@test.com" };
            var response = new DtoAdminResponse { Id = 3, Username = "admin3", Email = "admin3@test.com" };
            _adminServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(response);

            // Act
            var result = await _adminServiceMock.Object.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("admin3", result.Username);
        }

        [Fact]
        public async Task ValidateLoginAsync_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            _adminServiceMock.Setup(s => s.ValidateLoginAsync("admin1", "pass")).ReturnsAsync(true);

            // Act
            var result = await _adminServiceMock.Object.ValidateLoginAsync("admin1", "pass");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenAdminDeleted()
        {
            // Arrange
            _adminServiceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _adminServiceMock.Object.DeleteAsync(1);

            // Assert
            Assert.True(result);
        }
    }
}
