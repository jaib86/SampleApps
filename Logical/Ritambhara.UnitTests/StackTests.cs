using Xunit;

namespace Ritambhara.UnitTests
{
    public class StackTests
    {
        [Theory]
        [InlineData("[](){}", true)]
        [InlineData("[{()}]", true)]
        [InlineData("[{(})]", false)]
        [InlineData("[{})]", false)]
        public void IsValidBracketsPatterns(string pattern, bool isValidPattern)
        {
            Assert.Equal(isValidPattern, StackExample.IsValidBracketsPatterns(pattern));
        }

        [Theory]
        [InlineData("There was a law", "law a was There")]
        [InlineData("in the city of Athence", "Athence of city the in")]
        public void TestGetReverseString(string inputText, string expectedResult)
        {
            // Act
            var actualResult = StackExample.GetReverseString(inputText);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}