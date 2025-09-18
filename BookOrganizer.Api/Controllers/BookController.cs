/******************************************************************************
 * Creator:     Daniel Guglielmo
 * Date:        08/27/2025
 * Description: Book controller file created to allow the fetching of data 
 *              from the Book table. 
 * ***************************************************************************/

using BookOrganizer.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookOrganizer.Api.Controllers
{
    /// <summary>
    /// Basic CRUD operations for books in database
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILibraryContext _context;

        /// <summary>
        /// Configure the context 
        /// </summary>
        /// <param name="context"></param>
        public BookController(AudiobookOrganizerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Return all books in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            var allBooks = await _context.Books.ToListAsync();
            if (allBooks == null || allBooks.Count == 0)
            {
                return NotFound();
            }
            return Ok(allBooks);
        }

        /// <summary>
        /// Get a specific book from your library using the Audiobook Organizer internal ID
        /// </summary>
        /// <param name="id">Audiobook Organizer Internal ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(long id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        /// <summary>
        /// Edit the information held about a specific book
        /// </summary>
        /// <param name="id">Audiobook Organizer Internal ID</param>
        /// <param name="book">Book with its altered data</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(long id, Book book)
        {
            if (id != book.OrganizerBookId)
            {
                return BadRequest("Book ID doesn't match ID in request");
            }

            var bookToUpdate = await _context.Books.FindAsync(id);
            if (bookToUpdate == null)
            {
                return NotFound("Book record not found");
            }
            bookToUpdate.Asin = book.Asin;
            bookToUpdate.AuthorKey = book.AuthorKey;
            bookToUpdate.CoverId = book.CoverId;
            bookToUpdate.FirstPublishYear = book.FirstPublishYear;
            bookToUpdate.OpenLibraryBookId = book.OpenLibraryBookId;
            bookToUpdate.OpenLibraryWorksLink = book.OpenLibraryWorksLink;
            bookToUpdate.PublishDate = book.PublishDate;
            bookToUpdate.Publisher = book.Publisher;
            bookToUpdate.SeriesName = book.SeriesName;
            bookToUpdate.Title = book.Title;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) //when (!BookExists(id))
            {
                if (!BookExists(id))
                {
                    return NotFound("ID not found");
                }
                else
                {
                    throw;
                }
                
            }
            return NoContent();
        }

        /// <summary>
        /// Add a new book to the database
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            var newBook = new Book
            {
                Asin = book.Asin,
                AuthorKey = book.AuthorKey,
                CoverId = book.CoverId,
                FirstPublishYear = book.FirstPublishYear,
                OpenLibraryBookId = book.OpenLibraryBookId,
                OpenLibraryWorksLink = book.OpenLibraryWorksLink,
                PublishDate = book.PublishDate,
                Publisher = book.Publisher,
                SeriesName = book.SeriesName,
                Title = book.Title
            };
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBook),
                new { id = book.OrganizerBookId },
                BookToDTO(newBook));
        }

        /// <summary>
        /// Remove a specific book
        /// </summary>
        /// <param name="id">Audiobook Organizer Internal ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Ensure that the specific book exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool BookExists(long id)
        {
            return _context.Books.Any(b => b.OrganizerBookId == id);
        }

        /// <summary>
        /// In case of future DTO
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        private static BookDTO BookToDTO(Book book) =>
            new BookDTO
            {
                Asin = book.Asin,
                AuthorKey = book.AuthorKey,
                CoverId = book.CoverId,
                FirstPublishYear = book.FirstPublishYear,
                OpenLibraryBookId = book.OpenLibraryBookId,
                OpenLibraryWorksLink = book.OpenLibraryWorksLink,
                OrganizerBookId = book.OrganizerBookId,
                PublishDate = book.PublishDate,
                Publisher = book.Publisher,
                SeriesName = book.SeriesName,
                Title = book.Title
            };
    }

}
