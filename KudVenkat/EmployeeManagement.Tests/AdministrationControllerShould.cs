using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EmployeeManagement.Tests
{
    public class AdministrationControllerShould
    {
        private readonly AdministrationController administrationController;
        private readonly Mock<RoleManager<IdentityRole>> mockRoleManager;
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;

        public AdministrationControllerShould()
        {
            this.mockRoleManager = new Mock<RoleManager<IdentityRole>>();
            this.mockUserManager = new Mock<UserManager<ApplicationUser>>();

            // Throwing InvalidProxyConstructorArgumentsException
            // Cannot instantiate proxy of class RoleManager
            this.administrationController = new AdministrationController(this.mockRoleManager.Object, this.mockUserManager.Object);
        }

        [Fact]
        public void ShouldReturnViewForCreateRole()
        {
            Assert.IsType<ViewResult>(this.administrationController.CreateRole());
        }
    }
}