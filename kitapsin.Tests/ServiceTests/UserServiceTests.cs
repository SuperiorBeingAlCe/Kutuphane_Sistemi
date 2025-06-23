using AutoMapper;
using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using kitapsin.Server.Repositories;
using kitapsin.Server.Services;
using Moq;

namespace kitapsin.Tests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _userService = new UserService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetByIdAsync_UserExists_ReturnsMappedUser()
        {
            // Arrange
            var user = new User { Id = 1, FullName = "superior", Email = "superior@example.com" };
            var response = new DtoUserResponse { Id = 1, FullName = "superior" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<DtoUserResponse>(user)).Returns(response);

            // Act
            var result = await _userService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("superior", result.FullName);
        }

        [Fact]
        public async Task GetByIdAsync_UserDoesNotExist_ThrowsMyCustomException()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<MyCustomException>(() => _userService.GetByIdAsync(999));
            Assert.Equal("Kullanýcý bulunamadý. Id=999", exception.Message);

         
           
        }

        [Fact]
        public async Task CreateAsync_InvalidEmail_ThrowsMyCustomException()
        {
            // Arrange
            var invalidDto = new DtoUserCreate
            {
                FullName = "Test User",
                Email = "notanemail",
                PhoneNumber = "123456"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<MyCustomException>(() => _userService.CreateAsync(invalidDto));
            Assert.Equal("Geçersiz email formatý.", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_ValidData_CreatesUserAndReturnsResponse()
        {
            // Arrange
            var dto = new DtoUserCreate
            {
                FullName = "Test User",
                Email = "test@example.com",
                PhoneNumber = "123456"
            };

            var entity = new User();
            var response = new DtoUserResponse { FullName = "Test User" };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());
            _mockRepo.Setup(r => r.GetLastCardNumberAsync()).ReturnsAsync("000000005");
            _mockRepo.Setup(r => r.GetByCardNumberAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            _mockMapper.Setup(m => m.Map<User>(dto)).Returns(entity);
            _mockMapper.Setup(m => m.Map<DtoUserResponse>(entity)).Returns(response);

            // Act
            var result = await _userService.CreateAsync(dto);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
            Assert.Equal("Test User", result.FullName);
        }
    }
}