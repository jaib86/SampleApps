using System.Collections.Generic;

namespace EmployeeManagement.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);

        IEnumerable<Employee> GetAllEmployee();

        Employee Add(Employee employee);

        Employee Delete(int id);

        Employee Update(Employee employeeChanges);
    }
}