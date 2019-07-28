using Xunit;

namespace EmployeeManagement.UITests
{
    public class EmployeeManagementApplicationTests : BaseUIWebDriver
    {
        [Fact]
        public void ShouldLoadApplicationPage_SmokeTest()
        {
            this.driver.Navigate().GoToUrl("http://localhost:56624");

            Assert.Equal("Employee List", this.driver.Title);
        }
    }
}