using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EmployeeManagement.UITests
{
    public class BaseUIWebDriver : IDisposable
    {
        private bool disposed;
        protected readonly IWebDriver webDriver;

        protected const string Email = "Jack@techjp.in";
        protected const string Password = "Jack@1234";

        public BaseUIWebDriver()
        {
            this.webDriver = new ChromeDriver(Environment.CurrentDirectory);
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
            this.webDriver.Quit();
            this.webDriver.Dispose();

            this.disposed = true;
        }

        ~BaseUIWebDriver()
        {
            this.Dispose(false);
        }
    }
}