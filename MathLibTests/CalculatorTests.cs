using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathLib.Tests
{
    [TestClass()]
    public class CalculatorTests
    {
        [TestMethod()]
        public void AddShouldReturnSomeOfValues()
        {
            var calculator = new Calculator();
            var result = calculator.Add(2, 3);
            Assert.AreEqual(5, result);
        }
    }
}