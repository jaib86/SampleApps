using EmployeeManagement.UITests.PageObjectModels;
using Xunit;

namespace EmployeeManagement.UITests
{
    public class LoginPageShouldRefactored : BaseUIWebDriver
    {
        private readonly LoginPage loginPage;

        public LoginPageShouldRefactored()
        {
            this.loginPage = new LoginPage(this.driver);
            this.loginPage.NavigateTo();
        }

        [Fact]
        public void IntegrateAllSteps()
        {
            this.LoadLoginPageWithCorrectPageTitle();
            this.ValidateRequiredEmail();
            this.ValidateRequiredPassword();
            this.ValidateValidEmail();
            this.ValidateInvalidLogin();
            this.LoginWithAllDetails();
        }

        [Fact]
        public void LoadLoginPageWithCorrectPageTitle()
        {
            Assert.Equal(LoginPage.PageTitle, this.loginPage.Driver.Title);
        }

        [Fact]
        public void ValidateRequiredEmail()
        {
            // Don't enter Email and Password
            this.loginPage.EnterLoginCredential(string.Empty, string.Empty);

            // Submit button
            this.loginPage.SubmitLoginPage();

            // Make some delay
            DelayForDemoVideo();

            // Validate Email
            Assert.Equal("The Email field is required.", this.loginPage.FirstErrorMessage);
        }

        [Fact]
        public void ValidateRequiredPassword()
        {
            // Enter Email but don't enter Password
            this.loginPage.EnterLoginCredential(Email, string.Empty);

            // Make some delay
            DelayForDemoVideo();

            // Submit button
            this.loginPage.SubmitLoginPage();

            // Make some delay
            DelayForDemoVideo();

            // Validate Password
            Assert.Equal("The Password field is required.", this.loginPage.FirstErrorMessage);
        }

        [Fact]
        public void ValidateValidEmail()
        {
            // Enter Invalid Email and Correct Password
            this.loginPage.EnterLoginCredential("ABC", Password);

            // Make some delay
            DelayForDemoVideo();

            // Submit button
            this.loginPage.SubmitLoginPage();

            // Make some delay
            DelayForDemoVideo();

            // Validate Password
            Assert.Equal("The Email field is not a valid e-mail address.", this.loginPage.FirstErrorMessage);
        }

        [Fact]
        public void ValidateInvalidLogin()
        {
            // Enter Email and Wrong Password
            this.loginPage.EnterLoginCredential(Email, "ABC");

            // Make some delay
            DelayForDemoVideo();

            // Submit button
            this.loginPage.SubmitLoginPage();

            // Make some delay
            DelayForDemoVideo();

            // Validate Invalid Login
            Assert.Equal("Invalid login attempt", this.loginPage.FirstErrorMessage);
        }

        [Fact]
        public void LoginWithAllDetails()
        {
            // Enter Email & Password
            this.loginPage.EnterLoginCredential(Email, Password);

            DelayForDemoVideo();

            // Submit button
            this.loginPage.SubmitLoginPage();

            DelayForDemoVideo();

            // Check whether redirect to employee list
            Assert.Equal("Employee List", this.loginPage.Driver.Title);
        }
    }
}