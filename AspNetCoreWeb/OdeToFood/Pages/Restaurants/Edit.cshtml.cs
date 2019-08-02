using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class EditModel : PageModel
    {
        private readonly IRestaurantData restaurantData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public Restaurant Restaurant { get; set; }

        public IEnumerable<SelectListItem> Cuisines { get; set; }

        public EditModel(IRestaurantData restaurantData, IHtmlHelper htmlHelper)
        {
            this.restaurantData = restaurantData;
            this.htmlHelper = htmlHelper;
        }

        public IActionResult OnGet(int? restaurantId)
        {
            this.Cuisines = this.htmlHelper.GetEnumSelectList<CuisineType>();
            if (restaurantId.HasValue)
            {
                this.Restaurant = this.restaurantData.GetById(restaurantId.Value);
            }
            else
            {
                this.Restaurant = new Restaurant();
            }
            if (this.Restaurant == null)
            {
                return this.RedirectToPage("./NotFound");
            }
            else
            {
                return this.Page();
            }
        }

        public IActionResult OnPost()
        {
            if (!this.ModelState.IsValid)
            {
                this.Cuisines = this.htmlHelper.GetEnumSelectList<CuisineType>();
                return this.Page();
            }
            else
            {
                if (this.Restaurant.Id > 0)
                {
                    this.restaurantData.Update(this.Restaurant);
                    this.TempData["Message"] = "Restaurant Updated!";
                }
                else
                {
                    this.restaurantData.Add(this.Restaurant);
                    this.TempData["Message"] = "New Restaurant Saved!";
                }
                this.restaurantData.Commit();
                return this.RedirectToPage("./Detail", new { restaurantId = this.Restaurant.Id });
            }
        }
    }
}