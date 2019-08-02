using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantData restaurantData;

        public RestaurantsController(IRestaurantData restaurantData)
        {
            this.restaurantData = restaurantData;
        }

        // GET: api/Restaurants
        [HttpGet]
        public IEnumerable<Restaurant> Get()
        {
            return this.restaurantData.GetRestaurantsByName();
        }

        // GET: api/Restaurants/5
        [HttpGet("{id}", Name = "Get")]
        public Restaurant Get(int id)
        {
            return this.restaurantData.GetById(id);
        }

        // POST: api/Restaurants
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Restaurants/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}