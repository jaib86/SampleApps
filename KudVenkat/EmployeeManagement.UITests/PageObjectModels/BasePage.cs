using System;
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

        public void NavigateTo()
        {
            var root = new Uri(this.Driver.Url).GetLeftPart(UriPartial.Authority);

            var url = $"{root}/{this.pagePath}";

            this.Driver.Navigate().GoToUrl(url);
        }
    }
}