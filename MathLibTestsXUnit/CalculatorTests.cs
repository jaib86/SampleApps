using MathLib;
using Xunit;

namespace MathLibTestsXUnit
{
    public class CalculatorTests
    {
        [Theory]
        [InlineData(5, 2, 3)]
        [InlineData(2, 5, -3)]
        [InlineData(10, 4, 6)]
        [InlineData(0, 5, -5)]
        public void AddShouldReturnSomeOfValues(int expected, int val1, int val2)
        {
            var calculator = new Calculator();
            var result = calculator.Add(val1, val2);
            Assert.Equal(expected, result);
        }
    }
}