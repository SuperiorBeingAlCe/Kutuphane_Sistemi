
using kitapsin.Server.Controllers;
using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Services;
using Moq;

namespace kitapsin.Tests.ControllerTests
{
    public class CategoryControllerTest
    {
        private readonly Mock<ICategoryService> _mockService;
        private readonly CategoryController _controller;

        public CategoryControllerTest()
        {
            _mockService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            var categories = new List<DtoCategoryResponse>
            {
                new() { Id = 1, Name = "Roman" },
                new() { Id = 2, Name = "Bilim" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

            var result = await _controller.GetAllAsync();

            Assert.Equal(2, ((List<DtoCategoryResponse>)result).Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCategory_WhenExists()
        {
            var category = new DtoCategoryResponse { Id = 1, Name = "Roman" };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(category);

            var result = await _controller.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Roman", result!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(999))
                .ThrowsAsync(new MyCustomException("Admin bulunamadı"));

            var result = await _controller.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task SearchByNameAsync_ReturnsMatchingCategories()
        {
            var list = new List<DtoCategoryResponse>
            {
                new() { Id = 3, Name = "Felsefe" }
            };

            _mockService.Setup(s => s.SearchByNameAsync("Felsefe")).ReturnsAsync(list);

            var result = await _controller.SearchByNameAsync("Felsefe");

            Assert.Single(result);
        }

        [Fact]
        public async Task CreateAsync_ReturnsCreatedCategory()
        {
            var createDto = new DtoCategoryCreate { Name = "Yeni Kategori" };
            var responseDto = new DtoCategoryResponse { Id = 5, Name = "Yeni Kategori" };

            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(responseDto);

            var result = await _controller.CreateAsync(createDto);

            Assert.Equal("Yeni Kategori", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenSuccessful()
        {
            var updateDto = new DtoCategoryUpdate { Name = "Güncellendi" };

            _mockService.Setup(s => s.UpdateAsync(1, updateDto)).ReturnsAsync(true);

            var result = await _controller.UpdateAsync(1, updateDto);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenFailed()
        {
            var updateDto = new DtoCategoryUpdate { Name = "Deneme" };

            _mockService.Setup(s => s.UpdateAsync(99, updateDto)).ReturnsAsync(false);

            var result = await _controller.UpdateAsync(99, updateDto);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenSuccess()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenFailure()
        {
            _mockService.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

            var result = await _controller.DeleteAsync(999);

            Assert.False(result);
        }

        [Fact]
        public async Task GetBooksByCategoryAsync_ReturnsBooksList()
        {
            var books = new List<DtoBookResponse>
            {
                new() { Id = 1, Title = "Sefiller" },
                new() { Id = 2, Title = "1984" }
            };

            _mockService.Setup(s => s.GetBooksByCategoryAsync(1)).ReturnsAsync(books);

            var result = await _controller.GetBooksByCategoryAsync(1);

            Assert.Equal(2, ((List<DtoBookResponse>)result).Count);
        }
    }
}
