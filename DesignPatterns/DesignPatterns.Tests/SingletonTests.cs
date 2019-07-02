using Factory;
using Xunit;
using Xunit.Abstractions;

namespace DesignPatterns.Tests
{
    public class SingletonTests
    {
        private readonly ITestOutputHelper output;

        public SingletonTests(ITestOutputHelper outputHelper)
        {
            this.output = outputHelper;
        }

        [Fact]
        public void ShouldCreateJustOneInstance()
        {
            SingletonClass first = SingletonClass.Instance;
            SingletonClass second = SingletonClass.Instance;
            this.output.WriteLine($"First instance hashcode: {first.GetHashCode()}");
            this.output.WriteLine($"Second instance hashcode: {second.GetHashCode()}");
            Assert.Same(first, second);
            first.SomeValue++;
            this.output.WriteLine($"First instance {nameof(SingletonClass.SomeValue)}: {first.SomeValue}");
            this.output.WriteLine($"Second instance {nameof(SingletonClass.SomeValue)}: {second.SomeValue}");
            Assert.Equal(first.SomeValue, second.SomeValue);
            second.SomeValue++;
            this.output.WriteLine($"First instance {nameof(SingletonClass.SomeValue)}: {first.SomeValue}");
            this.output.WriteLine($"Second instance {nameof(SingletonClass.SomeValue)}: {second.SomeValue}");
            Assert.Equal(first.SomeValue, second.SomeValue);
        }
    }
}