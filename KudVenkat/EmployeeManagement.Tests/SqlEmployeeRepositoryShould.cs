using System;
using EmployeeManagement.Models;
using Xunit;

namespace EmployeeManagement.Tests
{
    public class SqlEmployeeRepositoryShould
    {
        [Fact]
        public void ThrowArugumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SqlEmployeeRepository(null, null));
        }
    }
}