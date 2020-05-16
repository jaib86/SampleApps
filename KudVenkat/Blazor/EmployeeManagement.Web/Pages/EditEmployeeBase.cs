using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeManagement.Models;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;
using Microsoft.AspNetCore.Components;

namespace EmployeeManagement.Web.Pages
{
    public class EditEmployeeBase : ComponentBase
    {
        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        [Inject]
        public IDepartmentService DepartmentService { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Id { get; set; }

        private Employee Employee { get; set; } = new Employee();
        public EditEmployeeModel EditEmployeeModel { get; set; } = new EditEmployeeModel();

        public IEnumerable<Department> Departments { get; set; } = new List<Department>();

        protected override async Task OnInitializedAsync()
        {
            int.TryParse(Id, out int employeeId);

            if (employeeId != 0)
            {
                Employee = await EmployeeService.GetEmployee(employeeId);
            }
            else
            {
                Employee = new Employee
                {
                    DepartmentId = 1,
                    DateOfBirth = DateTime.Now,
                    PhotoPath = "images/nophoto.jpg"
                };
            }

            Departments = await DepartmentService.GetDepartments();

            Mapper.Map(Employee, EditEmployeeModel);

            ///EditEmployeeModel.EmployeeId = Employee.EmployeeId;
            ///EditEmployeeModel.FirstName = Employee.FirstName;
            ///EditEmployeeModel.LastName = Employee.LastName;
            ///EditEmployeeModel.Email = Employee.Email;
            ///EditEmployeeModel.ConfirmEmail = Employee.Email;
            ///EditEmployeeModel.DateOfBirth = Employee.DateOfBirth;
            ///EditEmployeeModel.Gender = Employee.Gender;
            ///EditEmployeeModel.PhotoPath = Employee.PhotoPath;
            ///EditEmployeeModel.DepartmentId = Employee.DepartmentId;
            ///EditEmployeeModel.Department = Employee.Department;
        }

        protected async Task HandleValidSubmit()
        {
            Mapper.Map(EditEmployeeModel, Employee);

            Employee result = null;

            if (Employee.EmployeeId != 0)
            {
                result = await EmployeeService.UpdateEmployee(Employee);
            }
            else
            {
                result = await EmployeeService.CreateEmployee(Employee);
            }

            if (result != null)
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}