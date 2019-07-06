using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.DataAccess
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Mary", Department = Dept.IT, Email = "mary@techjp.com" },
                new Employee { Id = 2, Name = "John", Department = Dept.HR, Email = "john@techjp.com" },
                new Employee { Id = 3, Name = "Sam", Department = Dept.Payroll, Email = "sam@techjp.com" },
                new Employee { Id = 4, Name = "Jack", Department = Dept.IT, Email = "jack@techjp.com" }
            );
        }
    }
}