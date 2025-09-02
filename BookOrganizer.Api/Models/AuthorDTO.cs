using System;
using System.Collections.Generic;

namespace BookOrganizer.Api.Models;

public partial class AuthorDTO
{
    public long OrganizerAuthorId { get; set; }

    public string? OpenLibraryAuthorId { get; set; }

    public string? AuthorName { get; set; }

    public string? AuthorImageId { get; set; }
}
