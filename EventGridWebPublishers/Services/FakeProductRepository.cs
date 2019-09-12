using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventGridWebPublishers.Services
{
    public class FakeProductRepository
    {
        public readonly List<Product> FakeSource = new List<Product>();

        public FakeProductRepository()
        {
            for (var i = 1; i <= 10; i++)
            {
                FakeSource.Add(new Product(i, $"product -- {i}"));
            }
        }
    }
}
