using System.Collections.Generic;
using System.Diagnostics;

namespace Factory
{
    public class CaliforniaPizza : IPizza
    {
        private IList<string> ingredients;

        public CaliforniaPizza(IList<string> ingredients)
        {
            this.ingredients = ingredients;
        }

        public void Bake()
        {
            Debug.WriteLine($"{nameof(CaliforniaPizza)} Pizza baked.");
        }

        public void Box()
        {
            Debug.WriteLine($"{nameof(CaliforniaPizza)} Pizza boxed.");
        }

        public void Cut()
        {
            Debug.WriteLine($"{nameof(CaliforniaPizza)} Pizza cut.");
        }
    }
}