using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace BathanysPiShop.iOS.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null)
            {
                this.Control.BackgroundColor = UIColor.FromRGB(30, 100, 30);
            }
        }
    }
}