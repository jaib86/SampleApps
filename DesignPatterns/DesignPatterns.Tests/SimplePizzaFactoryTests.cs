using System.Collections.Generic;
using Factory;
using Factory.SimpleFactory;
using Xunit;
using Xunit.Abstractions;

namespace DesignPatterns.Tests
{
    public class SimplePizzaFactoryTests
    {
        private readonly ITestOutputHelper output;

        public SimplePizzaFactoryTests(ITestOutputHelper outputHelper)
        {
            this.output = outputHelper;
        }

        [Fact]
        public void CreateNewYorkPizza()
        {
            IPizza pizza = SimplePizzaFactory.CreatePizza(PizzaType.NewYork, new List<string>());
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            Assert.NotNull(pizza as NewYorkPizza);
        }

        [Fact]
        public void CreateChicagoPizza()
        {
            IPizza pizza = SimplePizzaFactory.CreatePizza(PizzaType.Chicago, new List<string>());
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            Assert.NotNull(pizza as ChicagoPizza);
        }

        [Fact]
        public void CreateCaliforniaPizza()
        {
            IPizza pizza = SimplePizzaFactory.CreatePizza(PizzaType.California, new List<string>());
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            Assert.NotNull(pizza as CaliforniaPizza);
        }

        [Theory]
        [InlineData(PizzaType.California)]
        [InlineData(PizzaType.Chicago)]
        [InlineData(PizzaType.NewYork)]
        public void ShouldCreateSpecificPizza(PizzaType pizzaType)
        {
            IPizza pizza = SimplePizzaFactory.CreatePizza(pizzaType, new List<string>());
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");

            switch (pizzaType)
            {
                case PizzaType.California:
                    Assert.NotNull(pizza as CaliforniaPizza);
                    break;

                case PizzaType.Chicago:
                    Assert.NotNull(pizza as ChicagoPizza);
                    break;

                case PizzaType.NewYork:
                    Assert.NotNull(pizza as NewYorkPizza);
                    break;
            }
        }
    }
}