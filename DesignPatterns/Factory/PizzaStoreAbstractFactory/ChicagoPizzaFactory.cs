using System.Collections.Generic;
using Factory.FactoryMethod;

namespace Factory.AbstractFactory
{
    public class ChicagoPizzaFactory : IPizzaFactory
    {
        public IPizza CreatePizza(IList<string> ingredients)
        {
            return new ChicagoPizza(ingredients);
        }
    }
}