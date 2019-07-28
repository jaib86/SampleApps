using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EmployeeManagement.UITests.PageObjectModels
{
    internal class CreateEmployeePage : BasePage
    {
        private const string PagePath = "Home/Create";

        public CreateEmployeePage(ChromeDriver driver) : base(driver, PagePath)
        { }

        private IWebElement Name => this.Driver.FindElementById("Name");

        private IWebElement Email => this.Driver.FindElementById("Email");

        private IWebElement Department => this.Driver.FindElementById("Department");

        private IWebElement SumbitButton => this.Driver.FindElementByCssSelector(".btn, .btn-primary");

        private IWebElement FirstError => this.Driver.FindElementByCssSelector(".validation-summary-errors ul > li");

        public string FirstErrorMessage => this.FirstError?.Text;

        public void EnterEmployeeInfo(string name, string email, int department)
        {
            this.Name.Clear();
            this.Name.SendKeys(name);
            this.Email.Clear();
            this.Email.SendKeys(email);
            if (this.Department != null)
            {

            }
        }

        public void SubmitLoginPage()
        {
            this.SumbitButton.Click();
        }
    }
}