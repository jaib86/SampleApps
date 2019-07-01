using System.Collections.Generic;

namespace Factory.FactoryMethod
{
    public abstract class PizzaStore
    {
        public IPizza OrderPizza(IList<string> ingredients)
        {
            IPizza pizza = this.CreatePizza(ingredients);
            pizza.Bake();
            pizza.Cut();
            pizza.Box();
            return pizza;
        }

        public abstract IPizza CreatePizza(IList<string> ingredients);
    }

    public class NewYorkPizzaStore : PizzaStore
    {
        public override IPizza CreatePizza(IList<string> ingredients)
        {
            // This is tied to a specific pizza implementation
            return new NewYorkPizza(ingredients);
        }
    }

    public class ChicagoPizzaStore : PizzaStore
    {
        public override IPizza CreatePizza(IList<string> ingredients)
        {
            // This is tied to a specific pizza implementation
            return new ChicagoPizza(ingredients);
        }
    }

    public class CaliforniaPizzaStore : PizzaStore
    {
        public override IPizza CreatePizza(IList<string> ingredients)
        {
            // This is tied to a specific pizza implementation
            return new CaliforniaPizza(ingredients);
        }
    }
}