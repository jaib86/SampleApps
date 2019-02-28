using MathLib;
using Xunit;

namespace MathLibTestsXUnit
{
    public class CalculatorTests
    {
        [Fact]
        public void AddShouldReturnSomeOfValues()
        {
            var calculator = new Calculator();
            var result = calculator.Add(2, 3);
            Assert.Equal(5, result);
        }
    }
}