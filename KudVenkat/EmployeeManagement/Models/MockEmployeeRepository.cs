using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> employees;

        public MockEmployeeRepository()
        {
            this.employees = new List<Employee>()
            {
                new Employee { Id = 1, Name = "Mary", Department = Dept.HR, Email = "marry@test.com" },
                new Employee { Id = 2, Name = "John", Department = Dept.IT, Email = "john@test.com" },
                new Employee { Id = 3, Name = "Sam", Department = Dept.IT, Email = "sam@test.com" }
            };
        }

        public Employee GetEmployee(int id)
        {
            return this.employees.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return this.employees;
        }

        public Employee Add(Employee employee)
        {
            employee.Id = this.employees.Any() ? this.employees.Max(e => e.Id) + 1 : 1;
            this.employees.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            var employee = this.employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                this.employees.Remove(employee);
            }

            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = this.employees.FirstOrDefault(e => e.Id == employeeChanges?.Id);

            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }

            return employee;
        }
    }
}