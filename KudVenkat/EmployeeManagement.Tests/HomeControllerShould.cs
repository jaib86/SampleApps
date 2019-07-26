using System.Collections.Generic;
using System.Linq;
using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EmployeeManagement.Tests
{
    public class HomeControllerShould
    {
        private readonly Mock<IEmployeeRepository> mockEmployeeRepository;
        private readonly List<Employee> employees;
        private readonly Mock<ILogger<HomeController>> mockLogger;
        private readonly HomeController homeController;

        public HomeControllerShould()
        {
            this.mockEmployeeRepository = new Mock<IEmployeeRepository>();

            this.employees = new List<Employee>()
                {
                    new Employee { Id = 1, Name = "Employee 1" },
                    new Employee { Id = 2, Name = "Employee 2" }
                };

            this.mockEmployeeRepository.Setup(x => x.GetAllEmployees()).Returns(this.employees);

            this.mockLogger = new Mock<ILogger<HomeController>>();
            //this.mockLogger.Setup(x => x.LogTrace(It.IsAny<string>()));

            this.homeController = new HomeController(this.mockEmployeeRepository.Object, null, this.mockLogger.Object);
        }

        [Fact]
        public void ReturnViewForIndex()
        {
            IActionResult result = this.homeController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CallGetAllEmployeesMethodOnceForIndex()
        {
            this.homeController.Index();
            this.mockEmployeeRepository.Verify(x => x.GetAllEmployees(), Times.Once);
        }

        [Fact]
        public void PassEmployeesViewModelToIndexView()
        {
            IActionResult result = this.homeController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<List<Employee>>(viewResult.Model);
            Assert.Equal(this.employees, model);
        }

        [Fact]
        public void ReturnViewForCreate()
        {
            IActionResult result = this.homeController.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ReturnViewForCreateWhenInvalidModelState()
        {
            this.homeController.ModelState.AddModelError("", "Test model error");

            var employeeCreateViewModel = new EmployeeCreateViewModel { Name = "Jack" };
            IActionResult result = this.homeController.Create(employeeCreateViewModel);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsType<EmployeeCreateViewModel>(viewResult.Model);

            Assert.Equal(employeeCreateViewModel.Name, model.Name);
        }

        [Fact]
        public void NotSaveEmployeeWhenModelError()
        {
            this.homeController.ModelState.AddModelError("", "Test Error");

            this.homeController.Create(new EmployeeCreateViewModel { Name = "Jack" });

            this.mockEmployeeRepository.Verify(x => x.Add(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public void CreateEmployeeWhenValidModel()
        {
            var employeeCreateViewModel = new EmployeeCreateViewModel
            {
                Name = "Jack",
                Email = "jack@techjp.in",
                Department = Dept.IT
            };

            this.mockEmployeeRepository.Setup(x => x.Add(It.IsAny<Employee>())).Returns(new Employee
            {
                Id = 101,
                Name = employeeCreateViewModel.Name,
                Email = employeeCreateViewModel.Email,
                Department = employeeCreateViewModel.Department
            });

            this.homeController.Create(employeeCreateViewModel);

            this.mockEmployeeRepository.Verify(x => x.Add(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public void RedirectToDetailsActionAfterCreatingEmployee()
        {
            const int employeeId = 101;

            this.mockEmployeeRepository.Setup(x => x.Add(It.IsAny<Employee>())).Returns(new Employee { Id = employeeId });

            var actionResult = this.homeController.Create(new EmployeeCreateViewModel());

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(actionResult);

            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(HomeController.Details), redirectToActionResult.ActionName);

            var routeValueDictionary = Assert.IsType<RouteValueDictionary>(redirectToActionResult.RouteValues);

            // Check for single route value
            Assert.Single(routeValueDictionary);

            Assert.Equal("id", routeValueDictionary.Keys.First());
            Assert.Equal(employeeId, routeValueDictionary.Values.First());
        }

        [Fact]
        public void ReturnEmployeeNotFoundViewWhenEmployeeNotFoundForDetails()
        {
            this.mockEmployeeRepository.Setup(x => x.GetEmployee(It.IsAny<int>()))
                                       .Returns(default(Employee));

            var viewResult = this.homeController.Details(int.MinValue);

            Assert.Equal("EmployeeNotFound", viewResult.ViewName);

            var employeeId = Assert.IsType<int>(viewResult.Model);

            Assert.Equal(int.MinValue, employeeId);
        }

        [Fact]
        public void ReturnCorrectViewWhenEmployeeFoundForDetails()
        {
            this.mockEmployeeRepository.Setup(x => x.GetEmployee(It.IsAny<int>()))
                                       .Returns(new Employee { Id = 1, Name = "Jack" });

            var viewResult = this.homeController.Details(1);

            Assert.Null(viewResult.ViewName);

            var homeDetailsViewModel = Assert.IsType<HomeDetailsViewModel>(viewResult.Model);

            Assert.Equal("Employee Details", homeDetailsViewModel.PageTitle);
            Assert.Equal(1, homeDetailsViewModel.Employee.Id);
            Assert.Equal("Jack", homeDetailsViewModel.Employee.Name);
        }
    }
}