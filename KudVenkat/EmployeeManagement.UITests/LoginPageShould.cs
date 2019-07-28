using OpenQA.Selenium;
using Xunit;

namespace EmployeeManagement.UITests
{
    public class LoginPageShould : BaseUIWebDriver
    {
        [Fact]
        public void IntegrateAllSteps()
        {
            this.LoadLoginPageWithCorrectPageTitle();
            this.ValidateRequiredEmail();
            this.ValidateRequiredPassword();
            this.ValidateValidEmail();
            this.ValidateInvalidLogin();
            this.LoginWithAllDetails(string.Empty);
        }

        [Fact]
        public void LoadLoginPageWithCorrectPageTitle()
        {
            this.webDriver.Navigate().GoToUrl("http://localhost:56624/Account/Login");

            Assert.Equal("User Login", this.webDriver.Title);
        }

        [Fact]
        public void ValidateRequiredEmail()
        {
            this.webDriver.Navigate().GoToUrl("http://localhost:56624/Account/Login");

            // Don't enter Email and Password

            // Submit button
            this.webDriver.FindElement(By.CssSelector(".btn, .btn-primary")).Click();

            // Make some delay
            this.DelayForDemoVideo();

            // Validate Email
            IWebElement emailValidationError = this.webDriver.FindElement(By.CssSelector(".validation-summary-errors ul > li"));
            Assert.Equal("The Email field is required.", emailValidationError.Text);
        }

        [Fact]
        public void ValidateRequiredPassword()
        {
            this.webDriver.Navigate().GoToUrl("http://localhost:56624/Account/Login");

            // Enter Email
            IWebElement email = this.webDriver.FindElement(By.Name("Email"));
            email.SendKeys(BaseUIWebDriver.Email);

            // Make some delay
            this.DelayForDemoVideo();

            // Don't enter Password

            // Submit button
            this.webDriver.FindElement(By.CssSelector(".btn, .btn-primary")).Click();

            // Make some delay
            this.DelayForDemoVideo();

            // Validate Password
            IWebElement validationError = this.webDriver.FindElement(By.CssSelector(".validation-summary-errors ul > li"));
            Assert.Equal("The Password field is required.", validationError.Text);
        }

        [Fact]
        public void ValidateValidEmail()
        {
            this.webDriver.Navigate().GoToUrl("http://localhost:56624/Account/Login");

            // Enter Wrong Email
            this.webDriver.FindElement(By.Name("Email")).SendKeys("ABC");

            // Make some delay
            this.DelayForDemoVideo();

            // Enter Password
            this.webDriver.FindElement(By.Name("Password")).SendKeys(Password);

            // Make some delay
            this.DelayForDemoVideo();

            // Submit button
            this.webDriver.FindElement(By.CssSelector(".btn, .btn-primary")).Click();

            // Make some delay
            this.DelayForDemoVideo();

            // Validate Password
            IWebElement invalidError = this.webDriver.FindElement(By.CssSelector(".validation-summary-errors ul > li"));
            Assert.Equal("The Email field is not a valid e-mail address.", invalidError.Text);
        }

        [Fact]
        public void ValidateInvalidLogin()
        {
            this.webDriver.Navigate().GoToUrl("http://localhost:56624/Account/Login");

            // Enter Email
            this.webDriver.FindElement(By.Name("Email")).SendKeys(Email);

            // Make some delay
            this.DelayForDemoVideo();

            // Enter Wrong Password
            this.webDriver.FindElement(By.Name("Password")).SendKeys("ABC");

            // Make some delay
            this.DelayForDemoVideo();

            // Submit button
            this.webDriver.FindElement(By.CssSelector(".btn, .btn-primary")).Click();

            // Make some delay
            this.DelayForDemoVideo();

            // Validate Invalid Login
            IWebElement validationError = this.webDriver.FindElement(By.CssSelector(".validation-summary-errors ul > li"));
            Assert.Equal("Invalid login attempt", validationError.Text);
        }

        [Theory]
        [InlineData("")]
        [InlineData("/techjp")]
        public void LoginWithAllDetails(string routing)
        {
            this.webDriver.Navigate().GoToUrl($"http://localhost:56624{routing}/Account/Login");

            this.DelayForDemoVideo();

            // Apply Email
            this.webDriver.FindElement(By.Id("Email")).SendKeys(Email);

            this.DelayForDemoVideo();

            // Apply Password
            this.webDriver.FindElement(By.Name("Password")).SendKeys(Password);

            this.DelayForDemoVideo();

            // Submit button
            this.webDriver.FindElement(By.CssSelector(".btn, .btn-primary")).Click();

            this.DelayForDemoVideo();

            // Check whether redirect to employee list
            Assert.Equal("Employee List", this.webDriver.Title);
        }
    }
}