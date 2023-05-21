using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace FluxoDeCaixa.API.ObjectResults
{
    [DefaultStatusCode(StatusCodes.Status500InternalServerError)]
    public sealed class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult([ActionResultObjectValue] object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
