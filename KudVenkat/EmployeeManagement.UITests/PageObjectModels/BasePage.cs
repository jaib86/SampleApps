using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EmployeeManagement.UITests.PageObjectModels
{
    internal class BasePage
    {
        private readonly string pagePath;

        public ChromeDriver Driver { get; }

        public BasePage(ChromeDriver driver, string pagePath)
        {
            this.Driver = driver;
            this.pagePath = pagePath;
        }

        private IWebElement FirstError => this.Driver.FindElementByCssSelector(".validation-summary-errors ul > li");

        public string FirstErrorMessage => this.FirstError?.Text;

        public void NavigateTo()
        {
            var root = new Uri(this.Driver.Url).GetLeftPart(UriPartial.Authority);

            var url = $"{root}/{this.pagePath}";

            this.Driver.Navigate().GoToUrl(url);
        }
    }
}