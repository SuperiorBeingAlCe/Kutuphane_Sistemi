using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Services;
using Moq;

namespace kitapsin.Tests.ServiceTests
{
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryService> _mockService;

        public CategoryServiceTest()
        {
            _mockService = new Mock<ICategoryService>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsCategoryList()
        {
            // Arrange
            var categories = new List<DtoCategoryResponse>
                {
                    new DtoCategoryResponse { Id = 1, Name = "Roman" },
                    new DtoCategoryResponse { Id = 2, Name = "Bilim" }
                };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _mockService.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<DtoCategoryResponse>)result).Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCategory_WhenFound()
        {
            // Arrange
            var category = new DtoCategoryResponse { Id = 1, Name = "Roman" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(category);

            // Act
            var result = await _mockService.Object.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Roman", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            _mockService
     .Setup(s => s.GetByIdAsync(999))
     .ThrowsAsync(new MyCustomException("Kategori bulunamadı"));

            // Act
            var result = await _mockService.Object.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SearchByNameAsync_ReturnsMatchingCategories()
        {
            // Arrange
            var categories = new List<DtoCategoryResponse>
                {
                    new DtoCategoryResponse { Id = 1, Name = "Roman" }
                };
            _mockService.Setup(s => s.SearchByNameAsync("Roman")).ReturnsAsync(categories);

            // Act
            var result = await _mockService.Object.SearchByNameAsync("Roman");

            // Assert
            Assert.Single(result);
            Assert.Equal("Roman", ((List<DtoCategoryResponse>)result)[0].Name);
        }

        [Fact]
        public async Task CreateAsync_ReturnsCreatedCategory()
        {
            // Arrange
            var createDto = new DtoCategoryCreate { Name = "Felsefe" };
            var responseDto = new DtoCategoryResponse { Id = 3, Name = "Felsefe" };
            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(responseDto);

            // Act
            var result = await _mockService.Object.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Felsefe", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenUpdateSuccessful()
        {
            // Arrange
            var updateDto = new DtoCategoryUpdate { Name = "Tarih" };
            _mockService.Setup(s => s.UpdateAsync(1, updateDto)).ReturnsAsync(true);

            // Act
            var result = await _mockService.Object.UpdateAsync(1, updateDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenUpdateFails()
        {
            // Arrange
            var updateDto = new DtoCategoryUpdate { Name = "Tarih" };
            _mockService.Setup(s => s.UpdateAsync(99, updateDto)).ReturnsAsync(false);

            // Act
            var result = await _mockService.Object.UpdateAsync(99, updateDto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenDeleteSuccessful()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _mockService.Object.DeleteAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenDeleteFails()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _mockService.Object.DeleteAsync(99);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetBooksByCategoryAsync_ReturnsBooks()
        {
            // Arrange
            var books = new List<DtoBookResponse>
                {
                    new DtoBookResponse { Id = 1, Title = "Kitap 1" }
                };
            _mockService.Setup(s => s.GetBooksByCategoryAsync(1)).ReturnsAsync(books);

            // Act
            var result = await _mockService.Object.GetBooksByCategoryAsync(1);

            // Assert
            Assert.Single(result);
            Assert.Equal("Kitap 1", ((List<DtoBookResponse>)result)[0].Title);
        }
    }
}
