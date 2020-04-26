﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Components;

namespace EmployeeManagement.Web.Pages
{
    public class EmployeeListBase : ComponentBase
    {
        public IEnumerable<Employee> Employees { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(LoadEmployees);
        }

        private void LoadEmployees()
        {
            System.Threading.Thread.Sleep(3000);

            var e1 = new Employee
            {
                EmployeeId = 1,
                FirstName = "John",
                LastName = "Hastings",
                Email = "john@gmail.com",
                DateOfBirth = new DateTime(1980, 10, 5),
                Gender = Gender.Male,
                Department = new Department { DepartmentId = 1, DepartmentName = "IT" },
                PhotoPath = "images/john.png"
            };

            var e2 = new Employee
            {
                EmployeeId = 2,
                FirstName = "Sam",
                LastName = "Galloway",
                Email = "sam@gmail.com",
                DateOfBirth = new DateTime(1981, 12, 22),
                Gender = Gender.Male,
                Department = new Department { DepartmentId = 2, DepartmentName = "Admin" },
                PhotoPath = "images/sam.jpg"
            };

            var e3 = new Employee
            {
                EmployeeId = 3,
                FirstName = "Mary",
                LastName = "Smith",
                Email = "mary@gmail.com",
                DateOfBirth = new DateTime(1979, 11, 11),
                Gender = Gender.Female,
                Department = new Department { DepartmentId = 1, DepartmentName = "IT" },
                PhotoPath = "images/mary.png"
            };

            var e4 = new Employee
            {
                EmployeeId = 4,
                FirstName = "Sara",
                LastName = "Longway",
                Email = "sara@gmail.com",
                DateOfBirth = new DateTime(1982, 9, 23),
                Gender = Gender.Female,
                Department = new Department { DepartmentId = 3, DepartmentName = "Accounts" },
                PhotoPath = "images/sara.png"
            };

            Employees = new List<Employee> { e1, e2, e3, e4 };
        }
    }
}