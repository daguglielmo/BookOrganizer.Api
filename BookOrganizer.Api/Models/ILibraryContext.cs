using Microsoft.EntityFrameworkCore;

namespace BookOrganizer.Api.Models
{
    public interface ILibraryContext : IDisposable
    {
        DbSet<Author> Authors { get; set; }
        DbSet<Book> Books { get; set; }
        Task<int> SaveChangesAsync();
    }
}
