using System;
using System.Collections.Generic;
using EmployeeManagement.DataAccess;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Models
{
    public class SqlEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;
        private readonly ILogger<SqlEmployeeRepository> logger;

        public SqlEmployeeRepository(AppDbContext context, ILogger<SqlEmployeeRepository> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        public IEnumerable<Employee> GetAllEmployees()
        {
            return this.context.Employees;
        }

        public Employee GetEmployee(int id)
        {
            this.logger.LogTrace("Trace Log");
            this.logger.LogDebug("Debug Log");
            this.logger.LogInformation("Information Log");
            this.logger.LogWarning("Warning Log");
            this.logger.LogError("Error Log");
            this.logger.LogCritical("Critical Log");

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