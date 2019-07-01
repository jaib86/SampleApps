using System.Collections.Generic;

namespace Factory.FactoryMethod
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