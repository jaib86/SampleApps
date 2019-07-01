using System.Collections.Generic;

namespace Factory.AbstractFactory
{
    public abstract class PizzaStoreAbstractFactory
    {
        public IPizzaFactory PizzaFactory { get; }

        protected PizzaStoreAbstractFactory(IPizzaFactory factory)
        {
            this.PizzaFactory = factory;
        }

        public IPizza OrderPizza(IList<string> ingredients)
        {
            IPizza pizza = this.PizzaFactory.CreatePizza(ingredients);
            pizza.Bake();
            pizza.Cut();
            pizza.Box();
            return pizza;
        }
    }

    public class NewYorkPizzaStoreAbstractFactory : PizzaStoreAbstractFactory
    {
        public NewYorkPizzaStoreAbstractFactory() :
            this(new NewYorkPizzaFactory())
        { }

        public NewYorkPizzaStoreAbstractFactory(IPizzaFactory pizzaFactory)
            : base(pizzaFactory) { }
    }

    public class ChicagoPizzaStoreAbstractFactory : PizzaStoreAbstractFactory
    {
        public ChicagoPizzaStoreAbstractFactory() :
            this(new ChicagoPizzaFactory())
        { }

        public ChicagoPizzaStoreAbstractFactory(IPizzaFactory pizzaFactory)
            : base(pizzaFactory) { }
    }

    public class CaliforniaPizzaStoreAbstractFactory : PizzaStoreAbstractFactory
    {
        public CaliforniaPizzaStoreAbstractFactory() :
            this(new CaliforniaPizzaFactory())
        { }

        public CaliforniaPizzaStoreAbstractFactory(IPizzaFactory pizzaFactory)
            : base(pizzaFactory) { }
    }
}