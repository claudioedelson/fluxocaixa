using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Tests.Extensions
{
    public static class RandomizerExtensions
    {
        public static string JsonWebToken(this Randomizer randomizer)
            => randomizer.String2(2048, "_/-abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
    }
}
