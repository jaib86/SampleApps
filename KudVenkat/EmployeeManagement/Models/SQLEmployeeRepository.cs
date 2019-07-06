using System;
using System.Collections.Generic;
using EmployeeManagement.DataAccess;

namespace EmployeeManagement.Models
{
    public class SqlEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;

        public SqlEmployeeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Employee Add(Employee employee)
        {
            this.context.Employees.Add(employee);
            this.context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            var employee = this.context.Employees.Find(id);

            if (employee != null)
            {
                this.context.Employees.Remove(employee);
                this.context.SaveChanges();
            }

            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return this.context.Employees;
        }

        public Employee GetEmployee(int id)
        {
            return this.context.Employees.Find(id);
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = this.context.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return employeeChanges;
        }
    }
}