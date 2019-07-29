using System;
using System.Threading;
using OpenQA.Selenium.Chrome;

namespace EmployeeManagement.UITests
{
    public class BaseUIWebDriver : IDisposable
    {
        private bool disposed;
        protected readonly ChromeDriver driver;

        #region Constants

        protected const string BaseUrl = "http://localhost:56624/";
        internal const string Email = "Jack@techjp.in";
        internal const string Password = "Jack@1234";

        #endregion Constants

        public BaseUIWebDriver()
        {
            this.driver = new ChromeDriver(Environment.CurrentDirectory);

            this.driver.Navigate().GoToUrl(BaseUrl);
        }

        /// <summary>
        /// Brief delay to slow down browser interactions for
        /// demo video recording purposes
        /// </summary>
        internal static void DelayForDemoVideo(int milliseconds = 1000)
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