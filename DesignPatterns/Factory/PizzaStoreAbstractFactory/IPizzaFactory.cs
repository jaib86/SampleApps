using System.Collections.Generic;
using Factory.FactoryMethod;

namespace Factory.AbstractFactory
{
    public interface IPizzaFactory
    {
        IPizza CreatePizza(IList<string> ingredients);
    }
}