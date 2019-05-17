using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BathanysPiShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PieOverview : ContentPage
    {
        public PieOverview()
        {
            InitializeComponent();

            var pies = new List<Pie>
            {
                new Pie
                {
                    PieName = "Apple Pie",
                    Price = 12.95,
                    Description  = "Icing carrot cake jelly-o cheesecake.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/",
                    InStock = true
                },
                new Pie
                {
                    PieName = "Strawberry Cheese Cake",
                    Price = 12.95,
                    Description  = "Icing carrot cake jelly-o cheesecake.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/",
                    InStock = true
                },
                new Pie
                {
                    PieName = "Strawberry Pie",
                    Price = 12.95,
                    Description  = "Icing carrot cake jelly-o cheesecake.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/",
                    InStock = true
                },
                new Pie
                {
                    PieName = "Rhubarb Pie",
                    Price = 12.95,
                    Description  = "Icing carrot cake jelly-o cheesecake.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/",
                    InStock = true
                },
                new Pie
                {
                    PieName = "Blueberry Cheese Cake",
                    Price = 12.95,
                    Description  = "Icing carrot cake jelly-o cheesecake.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/",
                    InStock = true
                },
                new Pie
                {
                    PieName = "Cheese Cake",
                    Price = 12.95,
                    Description  = "Icing carrot cake jelly-o cheesecake.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/",
                    InStock = true
                },
                new Pie
                {
                    PieName = "Christmas Apple Pie",
                    Price = 12.95,
                    Description  = "Icing carrot cake jelly-o cheesecake.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/",
                    InStock = true
                },
                new Pie
                {
                    PieName = "Cranberry Pie",
                    Price = 12.95,
                    Description  = "Icing carrot cake jelly-o cheesecake.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/",
                    InStock = true
                },
                new Pie
                {
                    PieName = "Peach Pie",
                    Price = 12.95,
                    Description  = "Icing carrot cake jelly-o cheesecake.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/",
                    InStock = true
                },
                new Pie
                {
                    PieName = "Pumpkin Pie",
                    Price = 12.95,
                    Description  = "Icing carrot cake jelly-o cheesecake.",
                    ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/",
                    InStock = true
                }
            };

            this.PiesListView.ItemsSource = pies;
        }

        private void PiesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Pie selectedPie)
            {
                //await this.Navigation.PushAsync(new PieDetailView(selectedPie));
            }
        }
    }
}