using System.Collections.Generic;
using Factory.FactoryMethod;

namespace Factory.AbstractFactory
{
    public class CaliforniaPizzaFactory : IPizzaFactory
    {
        public IPizza CreatePizza(IList<string> ingredients)
        {
            return new CaliforniaPizza(ingredients);
        }
    }
}