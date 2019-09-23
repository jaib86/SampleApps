using System.Collections.Generic;
using Library.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api")]
    public class RootController : Controller
    {
        private readonly IUrlHelper urlHelper;

        public RootController(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if (mediaType == "application/vnd.nagarro.hateoas+json")
            {
                var links = new List<LinkDto>
                {
                    new LinkDto(this.urlHelper.Link("GetRoot", new { }), "self", "GET"),
                    new LinkDto(this.urlHelper.Link("GetAuthors", new { }), "authors", "GET"),
                    new LinkDto(this.urlHelper.Link("CreateAuthor", new { }), "create_author", "GET")
                };

                return this.Ok(links);
            }
            else
            {
                return this.NoContent();
            }
        }
    }
}