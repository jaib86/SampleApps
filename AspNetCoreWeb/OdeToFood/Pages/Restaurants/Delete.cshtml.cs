using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class DeleteModel : PageModel
    {
        private readonly IRestaurantData restaurantData;

        public Restaurant Restaurant { get; set; }

        public DeleteModel(IRestaurantData restaurantData)
        {
            this.restaurantData = restaurantData;
        }

        public IActionResult OnGet(int restaurantId)
        {
            this.Restaurant = this.restaurantData.GetById(restaurantId);

            if (this.Restaurant == null)
            {
                return this.RedirectToPage("./NotFound");
            }
            else
            {
                return this.Page();
            }
        }

        public IActionResult OnPost(int restaurantId)
        {
            var restaurant = this.restaurantData.Delete(restaurantId);
            this.restaurantData.Commit();

            if (restaurant == null)
            {
                return this.RedirectToPage("./NotFound");
            }
            else
            {
                this.TempData["Message"] = $"{restaurant.Name} deleted";
                return this.RedirectToPage("./List");
            }
        }
    }
}