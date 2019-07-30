using Xunit;

namespace Ritambhara.UnitTests
{
    public class LogicalTests
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 1)]
        [InlineData(4, 2)]
        [InlineData(5, 3)]
        [InlineData(6, 5)]
        [InlineData(7, 8)]
        [InlineData(8, 13)]
        [InlineData(9, 21)]
        [InlineData(10, 34)]
        public void FibonacciNumber(int nthNumber, int fibonacciNumber)
        {
            Assert.Equal(fibonacciNumber, LogicalExample.GetNthFibonacciNumber(nthNumber));
        }

        [Fact]
        public void Sort_NxM_Matrix()
        {
            // Assign
            int[,] numbers = new int[,] {
                { 10, 5, 3, 8, 11 },
                { 1, 4, 8, 9, 100},
                { 23, 80, 23, 23, 1 }
            };
            int[,] sortedNumbers = new int[,] {
                { 1, 1, 3, 4,5 },
                { 8, 8, 9, 10, 11 },
                { 23, 23, 23, 80, 100 }
            };

            // Act
            LogicalExample.Sort_NxM_Matrix(numbers);

            // Assert
            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                for (int j = 0; j < numbers.GetLength(1); j++)
                {
                    Assert.Equal(sortedNumbers[i, j], numbers[i, j]);
                }
            }
        }

        [Theory]
        [InlineData("0000", 0)]
        [InlineData("0001", 1)]
        [InlineData("0010", 2)]
        [InlineData("0011", 3)]
        [InlineData("0100", 4)]
        [InlineData("1000", 8)]
        [InlineData("1111", 15)]
        [InlineData("10001111", 143)]
        public void ConvertBinaryToDecimal(string binaryString, int decimalValue)
        {
            Assert.Equal(decimalValue, LogicalExample.ConvertBinaryToDecimal(binaryString));
        }

        [Fact]
        public void GetNumberFromSingleDigitArray1()
        {
            Assert.Equal(1324, LogicalExample.GetNumberFromSingleDigitArray(new[] { 1, 3, 2, 4 }));
        }

        [Fact]
        public void GetNumberFromSingleDigitArray2()
        {
            Assert.Equal(24571, LogicalExample.GetNumberFromSingleDigitArray(new[] { 2, 4, 5, 7, 1 }));
        }
    }
}