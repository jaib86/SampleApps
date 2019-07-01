using System.Collections.Generic;

namespace Factory.AbstractFactory
{
    public interface IPizzaFactory
    {
        IPizza CreatePizza(IList<string> ingredients);
    }
}