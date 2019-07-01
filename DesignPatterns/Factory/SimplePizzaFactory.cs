using System.Collections.Generic;

namespace Factory.SimpleFactory
{
    /// <summary>
    /// The Simple Factory Pattern
    /// * Not a "true" pattern
    /// * Encapsulate object creation in one place
    /// * Should be the only part of the application that refers to concrete classes
    /// * Reduces duplicate code by enforcing DRY
    /// </summary>
    public static class SimplePizzaFactory
    {
        public static IPizza CreatePizza(PizzaType type, IList<string> ingredients)
        {
            switch (type)
            {
                case PizzaType.NewYork:
                    return new NewYorkPizza(ingredients);

                case PizzaType.Chicago:
                    return new ChicagoPizza(ingredients);

                case PizzaType.California:
                    return new CaliforniaPizza(ingredients);

                default:
                    return null;
            }
        }
    }
}