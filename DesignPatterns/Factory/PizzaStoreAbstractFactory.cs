using System.Collections.Generic;
using Factory.FactoryMethod;

namespace Factory.AbstractFactory
{
    public abstract class PizzaStoreAbstractFactory
    {
        private readonly IPizzaFactory factory;

        protected PizzaStoreAbstractFactory(IPizzaFactory factory)
        {
            this.factory = factory;
        }

        public IPizza OrderPizza(IList<string> ingredients)
        {
            IPizza pizza = this.factory.CreatePizza(ingredients);
            pizza.Bake();
            pizza.Cut();
            pizza.Box();
            return pizza;
        }
    }
}