using System.Linq;
using EmployeeManagement.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace EmployeeManagement.Tests
{
    public class ValuesControllerShould
    {
        [Fact]
        public void ReturnValues()
        {
            var valuesController = new ValuesController();

            var result = valuesController.Get().ToArray();

            Assert.Equal(2, result.Length);
            Assert.Equal("value1", result[0]);
            Assert.Equal("value2", result[1]);
        }

        [Fact]
        public void ReturnBadRequest()
        {
            var valuesController = new ValuesController();

            IActionResult result = valuesController.Get(0);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal("Invalid request for id 0", badRequestResult.Value);
        }

        [Fact]
        public void StartJobOk()
        {
            var valuesController = new ValuesController();

            IActionResult result = valuesController.StartJob();

            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal("Batch Job Started", okResult.Value);
        }
    }
}