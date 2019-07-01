using System.Collections.Generic;

namespace Factory.SimpleFactory
{
    public class NewYorkPizza : IPizza
    {
        private readonly IList<string> ingredients;

        public NewYorkPizza(IList<string> ingredients)
        {
            this.ingredients = ingredients;
        }
    }
}