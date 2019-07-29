using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EmployeeManagement.UITests.PageObjectModels
{
    internal class CreateEmployeePage : BasePage
    {
        private const string PagePath = "Home/Create";
        internal const string PageTitle = "Create Employee";

        public CreateEmployeePage(ChromeDriver driver) : base(driver, PagePath)
        { }

        private IWebElement Name => this.Driver.FindElementById("Name");

        private IWebElement Email => this.Driver.FindElementById("Email");

        private IWebElement Department => this.Driver.FindElementById("Department");

        private IWebElement SumbitButton => this.Driver.FindElementByCssSelector("button[type=submit].btn.btn-primary");

        public void EnterEmployeeInfo(string name, string email, int department)
        {
            this.Name.Clear();
            this.Name.SendKeys(name);
            this.Email.Clear();
            this.Email.SendKeys(email);
            
            // TODO: Select Department Drop-down
        }

        public void SubmitCreateEmployeePage()
        {
            Debug.WriteLine($"Clicking '{this.SumbitButton.Text}' button...");
            this.SumbitButton.Click();
        }
    }
}