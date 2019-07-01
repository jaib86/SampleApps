using System.Collections.Generic;

namespace Factory.FactoryMethod
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
            throw new System.NotImplementedException();
        }

        public void Box()
        {
            throw new System.NotImplementedException();
        }

        public void Cut()
        {
            throw new System.NotImplementedException();
        }
    }
}