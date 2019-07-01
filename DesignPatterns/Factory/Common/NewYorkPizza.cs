using System.Collections.Generic;
using System.Diagnostics;

namespace Factory
{
    public class NewYorkPizza : IPizza
    {
        private IList<string> ingredients;

        public NewYorkPizza(IList<string> ingredients)
        {
            this.ingredients = ingredients;
        }

        public void Bake()
        {
            Debug.WriteLine($"{nameof(NewYorkPizza)} Pizza baked.");
        }

        public void Box()
        {
            Debug.WriteLine($"{nameof(NewYorkPizza)} Pizza boxed.");
        }

        public void Cut()
        {
            Debug.WriteLine($"{nameof(NewYorkPizza)} Pizza cut.");
        }
    }
}