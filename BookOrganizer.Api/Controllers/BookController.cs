using BookOrganizer.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookOrganizer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        [HttpGet(Name = "GetBook")]
        public IEnumerable<Book> Get()
        {
            using (var context = new AudiobookOrganizerContext(_configuration))
            {
                return context.Books.ToList();
            }
        }
    }
}
