using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BathanysPiShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterView : ContentPage
    {
        public RegisterView()
        {
            InitializeComponent();

            this.BirthDayDatePicker.Date = DateTime.Today;
            this.RegisterButton.IsEnabled = false;
        }

        private void UserNameEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.RegisterButton.IsEnabled = this.CheckCanRegister();
        }

        private void PasswordEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.RegisterButton.IsEnabled = this.CheckCanRegister();
        }

        private void BirthDayDatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            if (e.NewDate == DateTime.Today)
            {
                this.DisplayAlert("Alert", "You weren't born today I guess", "OK, true!");
            }
        }

        private async void RegisterButton_OnClicked(object sender, EventArgs e)
        {
            await this.DisplayAlert("Success", "You have registered successfully", "Done");
        }

        private bool CheckCanRegister()
        {
            if (this.UserNameEntry.Text?.Length > 0 && this.PasswordEntry.Text?.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}