﻿using Bogus;

namespace FluxoCaixa.Tests.Extensions
{
    public static class FakerExtensions
    {
        public static Faker<T> UsePrivateConstructor<T>(this Faker<T> faker) where T : class
            => faker.CustomInstantiator(_ => Activator.CreateInstance(typeof(T), nonPublic: true) as T);
    }
}
