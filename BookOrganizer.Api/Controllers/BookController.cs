using BookOrganizer.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookOrganizer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            using (var _context = new AudiobookOrganizerContext())
            {
                var allBooks = await _context.Books.ToListAsync();
                if (allBooks == null || allBooks.Count == 0)
                {
                    return NotFound();
                }
                return Ok(allBooks);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(long id)
        {
            using (var _context = new AudiobookOrganizerContext())
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUpdateBook(long id, Book book)
        {
            if (id != book.OrganizerBookId)
            {
                return BadRequest("Book ID doesn't match ID in request");
            }
            using (var _context = new AudiobookOrganizerContext())
            {
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
                catch (DbUpdateConcurrencyException) when (!BookExists(id))
                {
                    return NotFound("ID not found");
                }
                return NoContent();
            }
        }
        [HttpPost]
        public async Task<ActionResult<Book>> PostAddBook(Book book)
        {
            using (var _context = new AudiobookOrganizerContext())
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
                    Title = book.Title,
                };
                _context.Books.Add(newBook);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetBook),
                    new { id = book.OrganizerBookId },
                    BookToDTO(newBook));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRemoveBook(long id)
        {
            using (var _context = new AudiobookOrganizerContext())
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
        }

        private bool BookExists(long id)
        {
            using (var _context = new AudiobookOrganizerContext())
            {
                return _context.Books.Any(b => b.OrganizerBookId == id);
            }
        }

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
                Title = book.Title,
            };
    }

}
