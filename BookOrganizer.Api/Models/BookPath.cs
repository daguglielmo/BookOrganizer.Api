using System;
using System.Collections.Generic;

namespace BookOrganizer.Api.Models;

public partial class BookPath
{
    public long PathId { get; set; }

    public long? BookId { get; set; }

    public long? AuthorId { get; set; }
}
