using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BathanysPiShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private async void LoginButton_OnClick(object sender, EventArgs e)
        {
            this.Navigation.InsertPageBefore(new MainView(), this);
            await this.Navigation.PopAsync();
        }
    }
}