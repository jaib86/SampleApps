using Xunit;

namespace Ritambhara.UnitTests
{
    public class SortingTests
    {
        [Fact]
        public void BubbleSort()
        {
            int[] numbersForSorting = { 65, 37, 11, 20, 45, 84, 24, 14, 3 };
            int[] sortedNumbers = { 3, 11, 14, 20, 24, 37, 45, 65, 84 };
            int[] result = SortingAlgorithm.BubbleSort(numbersForSorting);

            Assert.Equal(sortedNumbers, result);
        }
    }
}