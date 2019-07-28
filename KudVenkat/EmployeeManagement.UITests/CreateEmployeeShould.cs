using EmployeeManagement.UITests.PageObjectModels;
using Xunit;

namespace EmployeeManagement.UITests
{
    public class CreateEmployeeShould : BaseUIWebDriver
    {
        private readonly CreateEmployeePage page;

        public CreateEmployeeShould()
        {
            this.page = new CreateEmployeePage(this.driver);
            this.page.NavigateTo();
        }

        [Fact]
        public void CreateNewEmployee()
        {
            Assert.Equal("Create Employee", this.page.Driver.Title);
        }
    }
}