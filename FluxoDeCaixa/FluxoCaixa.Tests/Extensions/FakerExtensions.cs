using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Tests.Extensions
{
    public static class FakerExtensions
    {
        public static Faker<T> UsePrivateConstructor<T>(this Faker<T> faker) where T : class
            => faker.CustomInstantiator(_ => Activator.CreateInstance(typeof(T), nonPublic: true) as T);
    }
}
