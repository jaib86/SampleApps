using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EmployeeManagement.UITests.PageObjectModels
{
    internal class LoginPage : BasePage
    {
        internal const string PagePath = "Account/Login";
        internal const string PageTitle = "User Login";

        public LoginPage(ChromeDriver driver) : base(driver, PagePath)
        { }

        private IWebElement Email => this.Driver.FindElementById("Email");

        private IWebElement Password => this.Driver.FindElementById("Password");

        private IWebElement RememberMe => this.Driver.FindElementById("RememberMe");

        private IWebElement SumbitButton => this.Driver.FindElementByCssSelector("button[type=submit].btn.btn-primary");

        public void EnterLoginCredential(string email, string password)
        {
            this.Email.Clear();
            this.Email.SendKeys(email);
            this.Password.Clear();
            this.Password.SendKeys(password);
        }

        public void SetRememberMe(bool check)
        {
            if (check ^ this.RememberMe.Selected)
            {
                this.RememberMe.Click();
            }

            Debug.WriteLine($"Remember Me: {this.RememberMe.Selected}");
        }

        public void SubmitLoginPage()
        {
            this.SumbitButton.Click();
        }

        public static void LoginUser(ChromeDriver driver, bool rememberMe = false)
        {
            if (driver.Title == PageTitle)
            {
                Debug.WriteLine("Logging user...");

                var loginPage = new LoginPage(driver);

                // Enter Email & Password
                loginPage.EnterLoginCredential(BaseUIWebDriver.Email, BaseUIWebDriver.Password);
                BaseUIWebDriver.DelayForDemoVideo();

                // Click Remember Me
                loginPage.SetRememberMe(rememberMe);
                BaseUIWebDriver.DelayForDemoVideo();

                // Submit button
                loginPage.SubmitLoginPage();
                BaseUIWebDriver.DelayForDemoVideo();
            }
        }
    }
}