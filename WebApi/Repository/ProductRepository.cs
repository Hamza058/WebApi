using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<Deneme> Get()
        {
            /* Fake Data */
            var products = new Faker<Deneme>("tr")
                .RuleFor(x => x.id, x => x.IndexFaker)
                .RuleFor(x => x.Name, x => x.Commerce.ProductName())
                .RuleFor(x => x.Surname, x => x.Commerce.ProductDescription());

            return products.Generate(20);
        }
    }
}
