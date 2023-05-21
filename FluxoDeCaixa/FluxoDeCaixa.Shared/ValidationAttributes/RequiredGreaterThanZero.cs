using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Shared.ValidationAttributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class RequiredGreaterThanZeroAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
            => value != null && int.TryParse(value.ToString(), out var result) && result > 0;
    }
}
