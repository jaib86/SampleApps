using System.Collections.Generic;
using Factory;
using Factory.FactoryMethod;
using Xunit;
using Xunit.Abstractions;

namespace DesignPatterns.Tests
{
    public class PizzaFactoryMethodTests
    {
        private readonly ITestOutputHelper output;

        public PizzaFactoryMethodTests(ITestOutputHelper outputHelper)
        {
            this.output = outputHelper;
        }

        [Fact]
        public void CreateNewYorkPizza()
        {
            // Assign
            PizzaStore pizzaStore = new NewYorkPizzaStore();
            // Act
            IPizza pizza = pizzaStore.CreatePizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            // Assert
            Assert.NotNull(pizza as NewYorkPizza);
        }

        [Fact]
        public void CreateChicagoPizza()
        {
            // Assign
            PizzaStore pizzaStore = new ChicagoPizzaStore();
            // Act
            IPizza pizza = pizzaStore.CreatePizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            // Assert
            Assert.NotNull(pizza as ChicagoPizza);
        }

        [Fact]
        public void CreateCaliforniaPizza()
        {
            // Assign
            PizzaStore pizzaStore = new CaliforniaPizzaStore();
            // Act
            IPizza pizza = pizzaStore.CreatePizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            // Assert
            Assert.NotNull(pizza as CaliforniaPizza);
        }

        [Theory]
        [InlineData(PizzaType.California)]
        [InlineData(PizzaType.Chicago)]
        [InlineData(PizzaType.NewYork)]
        public void ShouldCreateSpecificPizza(PizzaType pizzaType)
        {
            // Assign
            PizzaStore pizzaStore = null;
            switch (pizzaType)
            {
                case PizzaType.California:
                    pizzaStore = new CaliforniaPizzaStore();
                    break;

                case PizzaType.Chicago:
                    pizzaStore = new ChicagoPizzaStore();
                    break;

                case PizzaType.NewYork:
                    pizzaStore = new NewYorkPizzaStore();
                    break;
            }

            // Act
            IPizza pizza = pizzaStore.CreatePizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");

            // Assert
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