using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id < 1)
            {
                return this.BadRequest($"Invalid request for id {id}");
            }
            else
            {
                return this.Content($"value {id}");
            }
        }

        [HttpPost("StartJob")]
        public IActionResult StartJob()
        {
            return this.Ok("Batch Job Started");
        }
    }
}