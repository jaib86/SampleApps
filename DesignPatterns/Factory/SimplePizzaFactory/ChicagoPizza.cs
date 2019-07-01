using System.Collections.Generic;

namespace Factory.SimpleFactory
{
    public class ChicagoPizza : IPizza
    {
        private readonly IList<string> ingredients;

        public ChicagoPizza(IList<string> ingredients)
        {
            this.ingredients = ingredients;
        }
    }
}