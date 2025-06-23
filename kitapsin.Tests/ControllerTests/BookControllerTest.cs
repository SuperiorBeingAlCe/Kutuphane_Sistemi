using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kitapsin.Server.Controllers;
using kitapsin.Server.Dto;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace kitapsin.Tests.ControllerTests
{
    public class BookControllerTest
    {
        private readonly Mock<IBookService> _mockService;
        private readonly BookController _controller;

        public BookControllerTest()
        {
            _mockService = new Mock<IBookService>();
            _controller = new BookController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithBooks()
        {
            // Arrange
            var books = new List<DtoBookResponse> { new() { Id = 1, Title = "Test" } };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(books);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBooks = Assert.IsAssignableFrom<IEnumerable<DtoBookResponse>>(okResult.Value);
            Assert.Single(returnedBooks);
        }

        [Fact]
        public async Task GetById_ReturnsBook_WhenFound()
        {
            var book = new DtoBookResponse { Id = 1, Title = "Kitap" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(book);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBook = Assert.IsType<DtoBookResponse>(okResult.Value);
            Assert.Equal(1, returnedBook.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((DtoBookResponse)null!);

            var result = await _controller.GetById(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task SearchByTitle_ReturnsResults()
        {
            var books = new List<DtoBookResponse> { new() { Id = 2, Title = "Deneme" } };
            _mockService.Setup(s => s.SearchByTitleAsync("Deneme")).ReturnsAsync(books);

            var result = await _controller.SearchByTitle("Deneme");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBooks = Assert.IsAssignableFrom<IEnumerable<DtoBookResponse>>(okResult.Value);
            Assert.Single(returnedBooks);
        }

        [Fact]
        public async Task Create_ReturnsCreatedResult()
        {
            var dto = new DtoBookCreate { Title = "Yeni Kitap" };
            var created = new DtoBookResponse { Id = 3, Title = "Yeni Kitap" };
            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await _controller.Create(dto);

            var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returned = Assert.IsType<DtoBookResponse>(createdAt.Value);
            Assert.Equal(3, returned.Id);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccess()
        {
            var dto = new DtoBookUpdate { Title = "Güncel" };
            _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(true);

            var result = await _controller.Update(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenFailed()
        {
            var dto = new DtoBookUpdate { Title = "Hatalı" };
            _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(false);

            var result = await _controller.Update(1, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccess()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenFailed()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

            var result = await _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
