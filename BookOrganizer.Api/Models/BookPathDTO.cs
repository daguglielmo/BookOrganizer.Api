namespace BookOrganizer.Api.Models
{
    public class BookPathDTO
    {
        public long PathId { get; set; }

        public long? BookId { get; set; }

        public long? AuthorId { get; set; }
    }
}
