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
        public void RedirectToLoginPageWhenNotLoggedIn()
        {
            Assert.Equal(LoginPage.PageTitle, this.page.Driver.Title);
            Assert.EndsWith($"{LoginPage.PagePath}?ReturnUrl=%2FHome%2FCreate", this.page.Driver.Url);
        }

        [Fact]
        public void ReturnBackToCreateEmployeeAfterSuccessLogin()
        {
            if (this.page.Driver.Title == LoginPage.PageTitle
                && this.page.Driver.Url.EndsWith($"{LoginPage.PagePath}?ReturnUrl=%2FHome%2FCreate"))
            {
                LoginPage.LoginUser(this.page.Driver);
            }

            Assert.Equal(CreateEmployeePage.PageTitle, this.page.Driver.Title);
        }

        [Fact]
        public void ValidateName()
        {
            this.ReturnBackToCreateEmployeeAfterSuccessLogin();

            this.page.EnterEmployeeInfo(string.Empty, string.Empty, (int)Models.Dept.IT);

            DelayForDemoVideo();

            this.page.SubmitCreateEmployeePage();

            DelayForDemoVideo();

            Assert.Equal("The Name field is required.", this.page.FirstErrorMessage);
        }

        [Fact]
        public void ValidateEmail()
        {
            this.ReturnBackToCreateEmployeeAfterSuccessLogin();

            this.page.EnterEmployeeInfo("Jack", string.Empty, (int)Models.Dept.IT);

            DelayForDemoVideo();

            this.page.SubmitCreateEmployeePage();

            DelayForDemoVideo();

            Assert.Equal("The Office Email field is required.", this.page.FirstErrorMessage);
        }


        [Fact]
        public void ValidateDepartment()
        {
            this.ReturnBackToCreateEmployeeAfterSuccessLogin();

            this.page.EnterEmployeeInfo("Jack", Email, (int)Models.Dept.IT);

            DelayForDemoVideo();

            this.page.SubmitCreateEmployeePage();

            DelayForDemoVideo();

            Assert.Equal("The Department field is required.", this.page.FirstErrorMessage);
        }
    }
}