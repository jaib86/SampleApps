using System;
using System.IO;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
        {
            this.employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        public ViewResult Index()
        {
            return this.View(this.employeeRepository.GetAllEmployee());
        }

        public ViewResult Details(int? id)
        {
#if TEMP
            return this.View("Test", this.employeeRepository.GetEmployee(1));
            return this.View("~/MyViews/Test.cshtml", this.employeeRepository.GetEmployee(1));
            return this.View("../Test/Update", this.employeeRepository.GetEmployee(1));
            return this.View("../../MyViews/Test", this.employeeRepository.GetEmployee(1));
#else
            var homeDetailsViewModel = new HomeDetailsViewModel
            {
                Employee = this.employeeRepository.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };
            return this.View(homeDetailsViewModel);
#endif
        }

        public ViewResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    var uploadsFolder = Path.Combine(this.hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                var newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                this.employeeRepository.Add(newEmployee);
                return this.RedirectToAction(nameof(Details), new { id = newEmployee.Id });
            }
            else
            {
                return this.View();
            }
        }
    }
}