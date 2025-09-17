using BookOrganizer.Api.Models;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;

namespace BookOrganizerAPI.Test
{
    public class BookOrganizerApiUnitTest
    {
        private Mock<AudiobookOrganizerContext> _mockOneBookContext;
        private Mock<DbSet<Book>> _mockOneBookSet;
        private IQueryable<Book> mockDbSet;

        private List<Book> _mockBooks => new List<Book> 
        {
            new Book
                {
                    OrganizerBookId = 1,
                    Asin = "ABCDE12345",
                    AuthorKey = "12345ABCDEF",
                    CoverId = "1A2B3C4D5E",
                    FirstPublishYear = "1999",
                    OpenLibraryBookId = "FEDCBA54321",
                    OpenLibraryWorksLink = @"/works/OL44444444W",
                    PublishDate = new DateOnly(2020, 11, 12),
                    Publisher = "Johnny B. Good",
                    SeriesName = "Test Series",
                    Title = "Some Book Title"
                },
            new Book
                {
                    OrganizerBookId = 2,
                    Asin = "B0BX4PGF8B",
                    AuthorKey = "OL14953152A",
                    CoverId = "15116337",
                    FirstPublishYear = "2023",
                    OpenLibraryBookId = "OL43936333W",
                    OpenLibraryWorksLink = @"/works/OL43936333W",
                    PublishDate = new DateOnly(2023, 2, 28),
                    Publisher = "Logan Jacobs",
                    SeriesName = "Backyard Dungeon",
                    Title = "Backyard Dungeon 2"
                },
            new Book
                {
                    OrganizerBookId = 3,
                    Asin = "",
                    AuthorKey = "OL233594A",
                    CoverId = "603075",
                    FirstPublishYear = "1990",
                    OpenLibraryBookId = "OL7924103W",
                    OpenLibraryWorksLink = @"/works/OL7924103W",
                    PublishDate = new DateOnly(1990, 2, 15),
                    Publisher = "Tor Books",
                    SeriesName = "Wheel of Time (1)",
                    Title = "The Eye of the World"
                }
        };

        public BookOrganizerApiUnitTest()
        {
            _mockOneBookSet = new Mock<DbSet<Book>>();
            _mockOneBookContext = new Mock<AudiobookOrganizerContext>();

            var mockDbSet = new List<Book>
            {
                new Book
                {
                    OrganizerBookId = 1,
                    Asin = "ABCDE12345",
                    AuthorKey = "12345ABCDEF",
                    CoverId = "1A2B3C4D5E",
                    FirstPublishYear = "1999",
                    OpenLibraryBookId = "FEDCBA54321",
                    OpenLibraryWorksLink = @"/works/OL43936333W",
                    PublishDate = new DateOnly(2020, 11, 12),
                    Publisher = "Johnny B. Good",
                    SeriesName = "Test Series",
                    Title = "Some Book Title"
                }
            }.AsQueryable();

            //_mockOneBookSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(mockDbSet.Provider);
            //_mockOneBookSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(mockDbSet.Expression);
            //_mockOneBookSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(mockDbSet.ElementType);
            //_mockOneBookSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(() => mockDbSet.GetEnumerator());
            SetMockSet<Book>(_mockOneBookSet, mockDbSet);

            _mockOneBookContext.Setup(c => c.Books).Returns(_mockOneBookSet.Object);
        }

        private void SetMockSet<T>(Mock mocSet, IQueryable<T> mocData )
        {
            mocSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(mocData.Provider);
            mocSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(mocData.Expression);
            mocSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(mocData.ElementType);
            mocSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => mocData.GetEnumerator());
        }

        [Fact]
        public void OneBookInDatabaseTest()
        {
            // Arrange
            
            // Act
            var result = _mockOneBookContext.Object.Books.ToList();

            // Assert
            Assert.Collection<Book>(result);
        }
    }
}
