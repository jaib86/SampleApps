using System;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository repository;
        private readonly IMapper mapper;

        public CampsController(ICampRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCamps()
        {
            try
            {
                var results = await this.repository.GetAllCampsAsync();
                CampModel[] models = this.mapper.Map<CampModel[]>(results);
                return this.Ok(models);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}