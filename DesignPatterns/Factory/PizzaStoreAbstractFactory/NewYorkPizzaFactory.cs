using System.Collections.Generic;
using Factory.FactoryMethod;

namespace Factory.AbstractFactory
{
    public class NewYorkPizzaFactory : IPizzaFactory
    {
        public IPizza CreatePizza(IList<string> ingredients)
        {
            return new NewYorkPizza(ingredients);
        }
    }
}