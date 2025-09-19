/******************************************************************************
 * Creator:     Daniel Guglielmo
 * Date:        08/27/2025
 * Description: Author controller file created to allow the fetching of data 
 *              from the Author table. Used reference from "Programming APIs 
 *              with C# and .NET" Chapter 4 to allow for the 
 *              translation of comments into info on swagger page
 * ***************************************************************************/

using BookOrganizer.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookOrganizer.Api.Controllers
{
    /// <summary>
    /// Basic CRUD operations for authors in database
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ILibraryContext _context;

        /// <summary>
        /// Configure the context 
        /// </summary>
        /// <param name="context"></param>
        public AuthorController()
        {
            _context = new AudiobookOrganizerContext();
        }

        /// <summary>
        /// Return a list of all authors in the database
        /// </summary>
        /// <returns>List of Authors</returns>
        [HttpGet()]
        public async Task<ActionResult<List<Author>>> GetAllAuthors()
        {
            var allAuthors = await _context.Authors.ToListAsync();
            if (allAuthors == null || allAuthors.Count == 0)
            {
                return NotFound();
            }
            return Ok(allAuthors);
        }

        /// <summary>
        /// Get a specific author from your library using the Audiobook Organizer internal ID
        /// </summary>
        /// <param name="id">Audiobook Organizer Internal ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(long id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        /// <summary>
        /// Edit the information held about a specific author
        /// </summary>
        /// <param name="id">Audiobook Organizer Internal ID</param>
        /// <param name="author">author with its altered data</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(long id, Author author)
        {
            if (id != author.OrganizerAuthorId)
            {
                return BadRequest("Book ID doesn't match ID in request");
            }

            var authorToUpdate = await _context.Authors.FindAsync(id);
            if (authorToUpdate == null)
            {
                return NotFound("Author record not found");
            }

            authorToUpdate.OrganizerAuthorId = id;
            authorToUpdate.OpenLibraryAuthorId = author.OpenLibraryAuthorId;
            authorToUpdate.AuthorName = author.AuthorName;
            authorToUpdate.AuthorImageId = author.AuthorImageId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!AuthorExists(id))
            {
                return NotFound("ID not found");
            }
            return NoContent();
        }

        /// <summary>
        /// Add a new author to the database
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            var newauthor = new Author
            {
                OpenLibraryAuthorId = author.OpenLibraryAuthorId,
                AuthorName = author.AuthorName,
                AuthorImageId = author.AuthorImageId,
            };
            _context.Authors.Add(newauthor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAuthor),
                new { id = author.OrganizerAuthorId },
                AuthorToDTO(newauthor));
        }

        /// <summary>
        /// Remove a specific author
        /// </summary>
        /// <param name="id">Audiobook Organizer Internal ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(long id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Ensure that the specific author exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool AuthorExists(long id)
        {
            return _context.Authors.Any(b => b.OrganizerAuthorId == id);
        }

        /// <summary>
        /// In case of future DTO
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        private static AuthorDTO AuthorToDTO(Author author) =>
            new AuthorDTO
            {
                OpenLibraryAuthorId = author.OpenLibraryAuthorId,
                AuthorName = author.AuthorName,
                AuthorImageId = author.AuthorImageId
            };
    }
}
