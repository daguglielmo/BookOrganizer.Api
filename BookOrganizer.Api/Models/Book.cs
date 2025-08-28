using System;
using System.Collections.Generic;

namespace BookOrganizer.Api.Models;

public partial class Book
{
    public long OrganizerBookId { get; set; }

    public string? OpenLibraryBookId { get; set; }

    public string? FirstPublishYear { get; set; }

    public string? Title { get; set; }

    public string? AuthorKey { get; set; }

    public string? OpenLibraryWorksLink { get; set; }

    public string? Publisher { get; set; }

    public DateOnly? PublishDate { get; set; }

    public string? Asin { get; set; }

    public string? SeriesName { get; set; }

    public string? CoverId { get; set; }

    public virtual ICollection<BookPath> BookPaths { get; set; } = new List<BookPath>();
}
