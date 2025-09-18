/******************************************************************************
 * Creator:     Daniel Guglielmo
 * Date:        09/17/2025
 * Description: BookPath controller file created to allow the fetching of data 
 *              from the BookPath table. 
 * ***************************************************************************/
using BookOrganizer.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookOrganizer.Api.Controllers
{
    /// <summary>
    /// Basic CRUD operations for book paths in database
    /// </summary>
    /// <remarks>
    /// Configure the context 
    /// </remarks>
    /// <param name="context"></param>
    [ApiController]
    [Route("api/[controller]")]
    public class BookPathController(AudiobookOrganizerContext context) : ControllerBase
    {
        private readonly AudiobookOrganizerContext _context = context;

        /// <summary>
        /// Return all book paths in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<BookPath>>> GetAllBookPaths()
        {
            var allBookPaths = await _context.BookPaths.ToListAsync();
            if (allBookPaths == null || allBookPaths.Count == 0)
            {
                return NotFound();
            }
            return Ok(allBookPaths);
        }

        /// <summary>
        /// Get a specific book path from your library using the Audiobook Organizer internal ID
        /// </summary>
        /// <param name="id">Audiobook Organizer Internal ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookPath>> GetBookPath(long id)
        {
            var bookPath = await _context.BookPaths.FindAsync(id);
            if (bookPath == null)
            {
                return NotFound();
            }
            return Ok(bookPath);
        }

        /// <summary>
        /// Update a specific book path in your library using the Audiobook Organizer internal ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookPath"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookPath(long id, BookPath bookPath)
        {
            if (id != bookPath.PathId)
            {
                return BadRequest("BookPath ID doesn't match ID in request");
            }
            var existingBookPath = await _context.BookPaths.FindAsync(id);


            if (existingBookPath == null)
            {
                return NotFound("BookPath record not found");
            }
            existingBookPath.AuthorId = bookPath.AuthorId;
            existingBookPath.BookId = bookPath.BookId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookPathExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        /// <summary>
        /// Create a new book path in your library
        /// </summary>
        /// <param name="bookPath"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<BookPath>> CreateBookPath(BookPath bookPath)
        {
            _context.BookPaths.Add(bookPath);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBookPath), new { id = bookPath.PathId }, BookPathToDTO(bookPath));
        }

        /// <summary>
        /// Delete a specific book path from your library using the Audiobook Organizer internal ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookPath(long id)
        {
            var bookPath = await _context.BookPaths.FindAsync(id);
            if (bookPath == null)
            {
                return NotFound();
            }
            _context.BookPaths.Remove(bookPath);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Check if a book path exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool BookPathExists(long id)
        {
            return _context.BookPaths.Any(e => e.PathId == id);
        }

        /// <summary>
        /// Convert BookPath to BookPathDTO
        /// </summary>
        /// <param name="bookPath"></param>
        /// <returns></returns>
        private static BookPathDTO BookPathToDTO(BookPath bookPath) =>
            new BookPathDTO
            {
                PathId = bookPath.PathId,
                BookId = bookPath.BookId,
                AuthorId = bookPath.AuthorId
            };
    }
}
