
using kitapsin.Server.Controllers;
using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace kitapsin.Tests.ControllerTests
{
    public class AdminControllerTest
    {
        private readonly Mock<IAdminService> _mockService;
        private readonly AdminController _controller;

        public AdminControllerTest()
        {
            _mockService = new Mock<IAdminService>();
            _controller = new AdminController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAdminList()
        {
            var admins = new List<DtoAdminResponse>
            {
                new() { Id = 1, Username = "admin1" },
                new() { Id = 2, Username = "admin2" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(admins);

            var result = await _controller.GetAllAsync();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<DtoAdminResponse>>(okResult.Value);
            Assert.Equal(2, ((List<DtoAdminResponse>)returnValue).Count);
        }

        [Fact]
        public async Task GetById_ReturnsAdmin_WhenFound()
        {
            var admin = new DtoAdminResponse { Id = 1, Username = "admin1" };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(admin);

            var result = await _controller.GetByIdAsync(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<DtoAdminResponse>(okResult.Value);
            Assert.Equal("admin1", returnValue.Username);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetByIdAsync(999))
             .ThrowsAsync(new MyCustomException("Admin bulunamadı"));

            var result = await _controller.GetByIdAsync(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetByUsername_ReturnsAdmin_WhenFound()
        {
            var admin = new DtoAdminResponse { Id = 3, Username = "admin3" };

            _mockService.Setup(s => s.GetByUsernameAsync("admin3")).ReturnsAsync(admin);

            var result = await _controller.GetByUsernameAsync("admin3");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<DtoAdminResponse>(okResult.Value);
            Assert.Equal(3, returnValue.Id);
        }

        [Fact]
        public async Task GetByUsername_ReturnsNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetByUsernameAsync("unknown"))
       .ThrowsAsync(new MyCustomException("Admin bulunamadı"));

            var result = await _controller.GetByUsernameAsync("unknown");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAdmin()
        {
            var createDto = new DtoAdminCreate { Username = "newAdmin", Password = "pass123" };
            var responseDto = new DtoAdminResponse { Id = 99, Username = "newAdmin" };

            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(responseDto);

            var result = await _controller.CreateAsync(createDto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<DtoAdminResponse>(createdResult.Value);
            Assert.Equal(99, returnValue.Id);
        }

        [Fact]
        public async Task Create_ThrowsCustomException_WhenUsernameExists()
        {
            var createDto = new DtoAdminCreate { Username = "duplicate", Password = "123" };

            _mockService.Setup(s => s.CreateAsync(createDto)).ThrowsAsync(new MyCustomException("Kullanıcı adı zaten var."));

            var exception = await Assert.ThrowsAsync<MyCustomException>(() => _controller.CreateAsync(createDto));
            Assert.Equal("Kullanıcı adı zaten var.", exception.Message);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithDtoAdminLogin_WhenValid()
        {
            var loginDto = new DtoAdminLogin { Username = "valid", Password = "123" };

            _mockService.Setup(s => s.ValidateLoginAsync("valid", "123")).ReturnsAsync(true);

            var result = await _controller.LoginAsync(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDto = Assert.IsType<DtoAdminLogin>(okResult.Value);

            Assert.Equal(loginDto.Username, returnedDto.Username);
            Assert.Equal(loginDto.Password, returnedDto.Password);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenInvalid()
        {
            var loginDto = new DtoAdminLogin { Username = "invalid", Password = "wrong" };

            _mockService.Setup(s => s.ValidateLoginAsync("invalid", "wrong")).ReturnsAsync(false);

            var result = await _controller.LoginAsync(loginDto);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            Assert.Equal("Geçersiz kullanıcı adı veya şifre.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccess()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteAsync(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenFail()
        {
            _mockService.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

            var result = await _controller.DeleteAsync(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
