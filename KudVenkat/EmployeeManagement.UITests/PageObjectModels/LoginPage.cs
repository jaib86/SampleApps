using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EmployeeManagement.UITests.PageObjectModels
{
    internal class LoginPage : BasePage
    {
        private const string PagePath = "Account/Login";

        public LoginPage(ChromeDriver driver) : base(driver, PagePath)
        { }

        private IWebElement Email => this.Driver.FindElementById("Email");

        private IWebElement Password => this.Driver.FindElementById("Password");

        private IWebElement SumbitButton => this.Driver.FindElementByCssSelector(".btn, .btn-primary");

        private IWebElement FirstError => this.Driver.FindElementByCssSelector(".validation-summary-errors ul > li");

        public string FirstErrorMessage => this.FirstError?.Text;

        public void EnterLoginCredential(string email, string password)
        {
            this.Email.Clear();
            this.Email.SendKeys(email);
            this.Password.Clear();
            this.Password.SendKeys(password);
        }

        public void SubmitLoginPage()
        {
            this.SumbitButton.Click();
        }
    }
}