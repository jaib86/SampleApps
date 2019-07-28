using System;
using System.Threading;
using OpenQA.Selenium.Chrome;

namespace EmployeeManagement.UITests
{
    public class BaseUIWebDriver : IDisposable
    {
        private bool disposed;
        protected readonly ChromeDriver driver;

        private const string URL = "http://localhost:56624/";
        protected const string Email = "Jack@techjp.in";
        protected const string Password = "Jack@1234";

        public BaseUIWebDriver()
        {
            this.driver = new ChromeDriver(Environment.CurrentDirectory);

            this.driver.Navigate().GoToUrl(URL);
        }

        /// <summary>
        /// Brief delay to slow down browser interactions for
        /// demo video recording purposes
        /// </summary>
        protected void DelayForDemoVideo(int milliseconds = 1000)
        {
            Thread.Sleep(milliseconds);
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources
            this.Dispose(true);
            // Suppress finalization
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;

            if (disposing)
            {
                // Free managed objects here.
            }

            // Free unmanaged objects here.
            this.driver.Quit();
            this.driver.Dispose();

            this.disposed = true;
        }

        ~BaseUIWebDriver()
        {
            this.Dispose(false);
        }
    }
}