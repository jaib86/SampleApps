using System.Collections.Generic;
using Factory;
using Factory.AbstractFactory;
using Xunit;
using Xunit.Abstractions;

namespace DesignPatterns.Tests
{
    public class PizzaStoreAbstractFactoryTests
    {
        private readonly ITestOutputHelper output;

        public PizzaStoreAbstractFactoryTests(ITestOutputHelper outputHelper)
        {
            this.output = outputHelper;
        }

        [Fact]
        public void OrderNewYorkPizzaFromNewYorkPizzaStore()
        {
            // Assign
            PizzaStoreAbstractFactory pizzaStore = new NewYorkPizzaStoreAbstractFactory();
            // Act
            IPizza pizza = pizzaStore.OrderPizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Factory: {pizzaStore.PizzaFactory.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            // Assert
            Assert.NotNull(pizza as NewYorkPizza);
        }

        [Fact]
        public void OrderNewYorkPizzaFromCaliforniaPizzaStore()
        {
            // Assign
            PizzaStoreAbstractFactory pizzaStore = new CaliforniaPizzaStoreAbstractFactory(new NewYorkPizzaFactory());
            // Act
            IPizza pizza = pizzaStore.OrderPizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Factory: {pizzaStore.PizzaFactory.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            // Assert
            Assert.NotNull(pizza as NewYorkPizza);
        }

        [Fact]
        public void OrderNewYorkPizzaFromChicagoPizzaStore()
        {
            // Assign
            PizzaStoreAbstractFactory pizzaStore = new ChicagoPizzaStoreAbstractFactory(new NewYorkPizzaFactory());
            // Act
            IPizza pizza = pizzaStore.OrderPizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Factory: {pizzaStore.PizzaFactory.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            // Assert
            Assert.NotNull(pizza as NewYorkPizza);
        }

        [Fact]
        public void OrderChicagoPizzaFromChicagoPizzaStore()
        {
            // Assign
            PizzaStoreAbstractFactory pizzaStore = new ChicagoPizzaStoreAbstractFactory();
            // Act
            IPizza pizza = pizzaStore.OrderPizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Factory: {pizzaStore.PizzaFactory.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            // Assert
            Assert.NotNull(pizza as ChicagoPizza);
        }

        [Fact]
        public void OrderChicagoPizzaFromNewYorkPizzaStore()
        {
            // Assign
            PizzaStoreAbstractFactory pizzaStore = new NewYorkPizzaStoreAbstractFactory(new ChicagoPizzaFactory());
            // Act
            IPizza pizza = pizzaStore.OrderPizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Factory: {pizzaStore.PizzaFactory.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            // Assert
            Assert.NotNull(pizza as ChicagoPizza);
        }

        [Fact]
        public void OrderChicagoPizzaFromCaliforniaPizzaStore()
        {
            // Assign
            PizzaStoreAbstractFactory pizzaStore = new CaliforniaPizzaStoreAbstractFactory(new ChicagoPizzaFactory());
            // Act
            IPizza pizza = pizzaStore.OrderPizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Factory: {pizzaStore.PizzaFactory.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            // Assert
            Assert.NotNull(pizza as ChicagoPizza);
        }

        [Fact]
        public void OrderCaliforniaPizzaFromCaliforniaPizzaStore()
        {
            // Assign
            PizzaStoreAbstractFactory pizzaStore = new CaliforniaPizzaStoreAbstractFactory();
            // Act
            IPizza pizza = pizzaStore.OrderPizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Factory: {pizzaStore.PizzaFactory.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            // Assert
            Assert.NotNull(pizza as CaliforniaPizza);
        }

        [Fact]
        public void OrderCaliforniaPizzaFromNewYorkPizzaStore()
        {
            // Assign
            PizzaStoreAbstractFactory pizzaStore = new NewYorkPizzaStoreAbstractFactory(new CaliforniaPizzaFactory());
            // Act
            IPizza pizza = pizzaStore.OrderPizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Factory: {pizzaStore.PizzaFactory.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            // Assert
            Assert.NotNull(pizza as CaliforniaPizza);
        }

        [Fact]
        public void OrderCaliforniaPizzaFromChicagoPizzaStore()
        {
            // Assign
            PizzaStoreAbstractFactory pizzaStore = new ChicagoPizzaStoreAbstractFactory(new CaliforniaPizzaFactory());
            // Act
            IPizza pizza = pizzaStore.OrderPizza(new List<string>());
            this.output.WriteLine($"Pizza Store: {pizzaStore.GetType()}");
            this.output.WriteLine($"Pizza Factory: {pizzaStore.PizzaFactory.GetType()}");
            this.output.WriteLine($"Pizza Type: {pizza.GetType()}");
            // Assert
            Assert.NotNull(pizza as CaliforniaPizza);
        }
    }
}