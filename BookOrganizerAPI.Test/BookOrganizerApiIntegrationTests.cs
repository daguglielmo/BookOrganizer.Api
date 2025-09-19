using BookOrganizer.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;

namespace BookOrganizerAPI.Test
{
    public class BookOrganizerApiIntegrationTests
    {
        [Fact]
        public async Task GetAllBooks()
        {
            // Arrange
            var api = new ApiWebApplicationFactory();

            // Act
            var response = await api.CreateClient().GetAsync("/api/book");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetSpecificBook()
        {
            // Arrange
            var api = new ApiWebApplicationFactory();

            // Act
            var response = await api.CreateClient().GetAsync("/api/book/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllAuthors()
        {
            // Arrange
            var api = new ApiWebApplicationFactory();

            // Act
            var response = await api.CreateClient().GetAsync("/api/Author");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetSpecificAuthor()
        {
            // Arrange
            var api = new ApiWebApplicationFactory();

            // Act
            var response = await api.CreateClient().GetAsync("/api/Author/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
