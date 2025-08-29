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

namespace BookOrganizer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        /// <summary>
        /// This API is for returning a list of all authors in the DB
        /// </summary>
        /// <returns>List of Authors</returns>
        [HttpGet(/*Name = "GetAuthor"*/)]
        public IEnumerable<Author> GetAll()
        {
            using (var context = new AudiobookOrganizerContext())
            {
                return context.Authors.ToList();
            }
        }

        
    }
}
