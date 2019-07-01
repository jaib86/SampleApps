using System.Collections.Generic;
using System.Diagnostics;

namespace Factory
{
    public class ChicagoPizza : IPizza
    {
        private IList<string> ingredients;

        public ChicagoPizza(IList<string> ingredients)
        {
            this.ingredients = ingredients;
        }

        public void Bake()
        {
            Debug.WriteLine($"{nameof(ChicagoPizza)} Pizza baked.");
        }

        public void Box()
        {
            Debug.WriteLine($"{nameof(ChicagoPizza)} Pizza boxed.");
        }

        public void Cut()
        {
            Debug.WriteLine($"{nameof(ChicagoPizza)} Pizza cut.");
        }
    }
}