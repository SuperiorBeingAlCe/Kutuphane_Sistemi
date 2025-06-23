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
    public class AuthorControllerTest
    {
        private readonly Mock<IAuthorService> _mockService;
        private readonly AuthorController _controller;

        public AuthorControllerTest()
        {
            _mockService = new Mock<IAuthorService>();
            _controller = new AuthorController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAuthors()
        {
            var authors = new List<DtoAuthorResponse> { new() { Id = 1, Name = "superior being" } };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(authors);

            var result = await _controller.GetAllAsync();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<DtoAuthorResponse>>(okResult.Value);
            Assert.Single(returned);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsAuthor_WhenExists()
        {
            var author = new DtoAuthorResponse { Id = 1, Name = "Üstün Varlık" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(author);

            var result = await _controller.GetByIdAsync(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<DtoAuthorResponse>(okResult.Value);
            Assert.Equal("Üstün Varlık", returned.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenNotExists()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((DtoAuthorResponse)null!);

            var result = await _controller.GetByIdAsync(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task SearchByTitleAsync_ReturnsFilteredAuthors()
        {
            var authors = new List<DtoAuthorResponse> { new() { Id = 2, Name = "Sorgulayan Zihin" } };
            _mockService.Setup(s => s.SearchByTitleAsync("Sorgu")).ReturnsAsync(authors);

            var result = await _controller.SearchByTitleAsync("Sorgu");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<DtoAuthorResponse>>(okResult.Value);
            Assert.Single(returned);
        }

        [Fact]
        public async Task CreateAsync_ReturnsCreatedAuthor()
        {
            var dto = new DtoAuthorCreate { Name = "Yeni Yazar" };
            var created = new DtoAuthorResponse { Id = 3, Name = "Yeni Yazar" };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await _controller.CreateAsync(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returned = Assert.IsType<DtoAuthorResponse>(createdResult.Value);
            Assert.Equal(3, returned.Id);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNoContent_WhenSuccessful()
        {
            var dto = new DtoAuthorUpdate { Name = "Güncel Yazar" };
            _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(true);

            var result = await _controller.UpdateAsync(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNotFound_WhenFails()
        {
            var dto = new DtoAuthorUpdate { Name = "Olmayan" };
            _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(false);

            var result = await _controller.UpdateAsync(1, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNoContent_WhenSuccessful()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteAsync(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNotFound_WhenFails()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

            var result = await _controller.DeleteAsync(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetBooksByAuthorIdAsync_ReturnsBooks()
        {
            var books = new List<DtoBookResponse>
            {
                new() { Id = 1, Title = "Yazarın Kitabı" }
            };

            _mockService.Setup(s => s.GetBooksByAuthorIdAsync(1)).ReturnsAsync(books);

            var result = await _controller.GetBooksByAuthorIdAsync(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBooks = Assert.IsAssignableFrom<IEnumerable<DtoBookResponse>>(okResult.Value);
            Assert.Single(returnedBooks);
        }
    }
}
