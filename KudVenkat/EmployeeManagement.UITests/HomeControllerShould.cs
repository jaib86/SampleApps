using Xunit;

namespace EmployeeManagement.UITests
{
    public class HomeControllerShould : BaseUIWebDriver
    {
        [Fact]
        public void NavigateToRootUrl()
        {
            this.driver.Navigate().GoToUrl(BaseUrl);

            Assert.Equal("Employee List", this.driver.Title);
        }
    }
}