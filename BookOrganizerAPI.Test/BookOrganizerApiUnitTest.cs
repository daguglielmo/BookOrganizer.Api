using BookOrganizer.Api.Models;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;

namespace BookOrganizerAPI.Test
{
    public class BookOrganizerApiUnitTest
    {
        private Mock<AudiobookOrganizerContext>? _mockBookContext;
        private Mock<DbSet<Book>>? _mockBookSet;
        private Mock<DbSet<Author>>? _mockAuthorsSet;


        #region Mock Data
        private IQueryable<Book> mockDbOneBookSet => new List<Book>
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
            }
        }.AsQueryable();

        private IQueryable<Author> mockDbOneAuthorSet => new List<Author>
        {
            new Author
            {
                OrganizerAuthorId = 0,
                OpenLibraryAuthorId = "OK1234567A",
                AuthorName = "Billy Boberson",
                AuthorImageId = "87654321"
            }
        }.AsQueryable();

        private IQueryable<Book> mockDbBooksSet => new List<Book> 
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
        }.AsQueryable();

        private IQueryable<Author> mockDbAuthorsSet => new List<Author>
        {
            new Author {
                OrganizerAuthorId = 0,
                OpenLibraryAuthorId = "OL1394865A",
                AuthorName = "Brandon Sanderson",
                AuthorImageId = "14851192"
            },
            new Author {
                OrganizerAuthorId = 1,
                OpenLibraryAuthorId = "OL14953152A",
                AuthorName = "Logan Jacobs",
                AuthorImageId = ""
            },
            new Author {
                OrganizerAuthorId = 2,
                OpenLibraryAuthorId = "OL233594A",
                AuthorName = "Robert Jordan",
                AuthorImageId = "6680409"
            }
        }.AsQueryable();
        #endregion

        #region Setup
        private void BookOrganizerApiUnitTestSingles()
        {
            _mockBookSet = new Mock<DbSet<Book>>();
            _mockAuthorsSet = new Mock<DbSet<Author>>();
            _mockBookContext = new Mock<AudiobookOrganizerContext>();

            SetMockSet<Book>(_mockBookSet, mockDbOneBookSet);
            SetMockSet<Author>(_mockAuthorsSet, mockDbOneAuthorSet);

            _mockBookContext.Setup(c => c.Books).Returns(_mockBookSet.Object);
            _mockBookContext.Setup(c => c.Authors).Returns(_mockAuthorsSet.Object);
        }

        private void BookOrganizerApiUnitTestMultiples()
        {
            _mockBookSet = new Mock<DbSet<Book>>();
            _mockAuthorsSet = new Mock<DbSet<Author>>();
            _mockBookContext = new Mock<AudiobookOrganizerContext>();

            SetMockSet<Book>(_mockBookSet, mockDbBooksSet);
            SetMockSet<Author>(_mockAuthorsSet, mockDbAuthorsSet);

            _mockBookContext.Setup(c => c.Books).Returns(_mockBookSet.Object);
            _mockBookContext.Setup(c => c.Authors).Returns(_mockAuthorsSet.Object);
        }

        private void SetMockSet<T>(Mock mocSet, IQueryable<T> mocData )
        {
            mocSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(mocData.Provider);
            mocSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(mocData.Expression);
            mocSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(mocData.ElementType);
            mocSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => mocData.GetEnumerator());
        }
        #endregion

        #region Context Tests
        [Fact]
        public void OneBookInDatabaseTest()
        {
            // Arrange
            BookOrganizerApiUnitTestSingles();

            // Act
            var result = _mockBookContext.Object.Books.ToList();

            // Assert
            Assert.NotEmpty(result);
            // Because of class construction, Equal does not work here.
            Assert.Equivalent(mockDbOneBookSet, result);
            Assert.Single(result);
        }

        [Fact]
        public void OneAuthorInDatabaseTest()
        {
            // Arrange
            BookOrganizerApiUnitTestSingles();

            // Act
            var result = _mockBookContext.Object.Authors.ToList();

            // Assert
            Assert.NotEmpty(result);
            // Because it is a class, Equal does not work here.
            Assert.Equivalent(mockDbOneAuthorSet, result);
            Assert.Single(result);
        }

        [Fact]
        public void MultipleAuthorsInDatabaseTest()
        {
            // Arrange
            BookOrganizerApiUnitTestMultiples();

            // Act
            var result = _mockBookContext.Object.Authors.ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Collection(result,
                item =>
                {
                    Assert.Equal("Brandon Sanderson", item.AuthorName);
                    Assert.Equal("14851192", item.AuthorImageId);
                    Assert.Equal("OL1394865A", item.OpenLibraryAuthorId);
                    Assert.Equal(0, item.OrganizerAuthorId);
                },
                item =>
                {
                    Assert.Equal("Logan Jacobs", item.AuthorName);
                    Assert.Equal("", item.AuthorImageId);
                    Assert.Equal("OL14953152A", item.OpenLibraryAuthorId);
                    Assert.Equal(1, item.OrganizerAuthorId);
                },
                item =>
                {
                    Assert.Equal("Robert Jordan", item.AuthorName);
                    Assert.Equal("6680409", item.AuthorImageId);
                    Assert.Equal("OL233594A", item.OpenLibraryAuthorId);
                    Assert.Equal(2, item.OrganizerAuthorId);
                }
            );
        }

        [Fact]
        public void MultipleBooksInDatabaseTest()
        {
            // Arrange
            BookOrganizerApiUnitTestMultiples();

            // Act
            var result = _mockBookContext.Object.Books.ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Collection(result,
                item =>
                {
                    Assert.Equal("Some Book Title", item.Title);
                    Assert.Equal(1, item.OrganizerBookId);
                    Assert.Equal("ABCDE12345", item.Asin);
                    Assert.Equal("12345ABCDEF", item.AuthorKey);
                    Assert.Equal("1A2B3C4D5E", item.CoverId);
                    Assert.Equal("1999", item.FirstPublishYear);
                    Assert.Equal("FEDCBA54321", item.OpenLibraryBookId);
                    Assert.Equal(@"/works/OL44444444W", item.OpenLibraryWorksLink);
                    Assert.Equal(new DateOnly(2020, 11, 12), item.PublishDate);
                    Assert.Equal("Johnny B. Good", item.Publisher);
                    Assert.Equal("Test Series", item.SeriesName);
                },
                item =>
                {
                    Assert.Equal("Backyard Dungeon 2", item.Title);
                    Assert.Equal(2, item.OrganizerBookId);
                    Assert.Equal("B0BX4PGF8B", item.Asin);
                    Assert.Equal("OL14953152A", item.AuthorKey);
                    Assert.Equal("15116337", item.CoverId);
                    Assert.Equal("2023", item.FirstPublishYear);
                    Assert.Equal("OL43936333W", item.OpenLibraryBookId);
                    Assert.Equal(@"/works/OL43936333W", item.OpenLibraryWorksLink);
                    Assert.Equal(new DateOnly(2023, 2, 28), item.PublishDate);
                    Assert.Equal("Logan Jacobs", item.Publisher);
                    Assert.Equal("Backyard Dungeon", item.SeriesName);
                },
                item =>
                {
                    Assert.Equal("The Eye of the World", item.Title);
                    Assert.Equal(3, item.OrganizerBookId);
                    Assert.Equal("", item.Asin);
                    Assert.Equal("OL233594A", item.AuthorKey);
                    Assert.Equal("603075", item.CoverId);
                    Assert.Equal("1990", item.FirstPublishYear);
                    Assert.Equal("OL7924103W", item.OpenLibraryBookId);
                    Assert.Equal(@"/works/OL7924103W", item.OpenLibraryWorksLink);
                    Assert.Equal(new DateOnly(1990, 2, 15), item.PublishDate);
                    Assert.Equal("Tor Books", item.Publisher);
                    Assert.Equal("Wheel of Time (1)", item.SeriesName);
                }
            );

        }
        #endregion
    }
}
