using Microsoft.EntityFrameworkCore;

namespace BookOrganizer.Api.Models
{
    public interface ILibraryContext : IDisposable
    {
        DbSet<Author> Authors { get; set; }
        DbSet<Book> Books { get; set; }
        DbSet<BookPath> BookPaths { get; set; }
        Task<int> SaveChangesAsync();
    }
}
