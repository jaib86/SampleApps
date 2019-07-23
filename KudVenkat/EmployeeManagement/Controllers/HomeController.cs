﻿using System;
using System.IO;
using System.Linq;
using EmployeeManagement.Models;
using EmployeeManagement.Utilities;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILogger<HomeController> logger;

        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment, ILogger<HomeController> logger)
        {
            this.employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            this.logger.LogTrace("Trace Log");
            this.logger.LogDebug("Debug Log");
            this.logger.LogInformation("Information Log");
            this.logger.LogWarning("Warning Log");
            this.logger.LogError("Error Log");
            this.logger.LogCritical("Critical Log");

            return this.View(this.employeeRepository.GetAllEmployee());
        }

        [IsMobile(2)]
        [AllowAnonymous]
        [Route("Home/Index")]
        public ViewResult AndroidIndex()
        {
            var employees = this.employeeRepository.GetAllEmployee().ToList();
            employees.Insert(0, new Employee { Id = 0, Name = "Android 2", Email = "android.index@techjp.in" });
            return this.View(nameof(Index), employees);
        }

        [IsMobile(1)]
        [AllowAnonymous]
        [Route("Home/Index")]
        public ViewResult AndroidIndex2()
        {
            var employees = this.employeeRepository.GetAllEmployee().ToList();
            employees.Insert(0, new Employee { Id = 0, Name = "Android 1", Email = "android.index@techjp.in" });
            return this.View(nameof(Index), employees);
        }

        [AllowAnonymous]
        public ViewResult Details(int id)
        {
#if TEMP
            return this.View("Test", this.employeeRepository.GetEmployee(1));
            return this.View("~/MyViews/Test.cshtml", this.employeeRepository.GetEmployee(1));
            return this.View("../Test/Update", this.employeeRepository.GetEmployee(1));
            return this.View("../../MyViews/Test", this.employeeRepository.GetEmployee(1));
#else
            var employee = this.employeeRepository.GetEmployee(id);

            if (employee == null)
            {
                return this.View("EmployeeNotFound", id);
            }
            else
            {
                var homeDetailsViewModel = new HomeDetailsViewModel
                {
                    Employee = employee,
                    PageTitle = "Employee Details"
                };
                return this.View(homeDetailsViewModel);
            }
#endif
        }

        [HttpGet]
        [Authorize]
        public ViewResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                string uniqueFileName = this.ProcessUploadedFile(model);

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

        [HttpGet]
        [Authorize]
        public ViewResult Edit(int id)
        {
            var employee = this.employeeRepository.GetEmployee(id);
            var employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return this.View(employeeEditViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var employee = this.employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                if (model.Photo != null)
                {
                    if (System.IO.File.Exists(Path.Combine(this.hostingEnvironment.ImagesWebRootPath(), model.ExistingPhotoPath)))
                    {
                        System.IO.File.Delete(Path.Combine(this.hostingEnvironment.ImagesWebRootPath(), model.ExistingPhotoPath));
                    }

                    string uniqueFileName = this.ProcessUploadedFile(model);
                    employee.PhotoPath = uniqueFileName;
                }

                this.employeeRepository.Update(employee);
                return this.RedirectToAction(nameof(Index));
            }
            else
            {
                return this.View();
            }
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                var uploadsFolder = Path.Combine(this.hostingEnvironment.ImagesWebRootPath());
                uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}