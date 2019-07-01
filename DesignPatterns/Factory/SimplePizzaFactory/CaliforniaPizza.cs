using System.Collections.Generic;

namespace Factory.SimpleFactory
{
    public class CaliforniaPizza : IPizza
    {
        private readonly IList<string> ingredients;

        public CaliforniaPizza(IList<string> ingredients)
        {
            this.ingredients = ingredients;
        }
    }
}