using FluxoDeCaixa.API.Models;
using FluxoDeCaixa.Shared.Messages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FluxoCaixa.Tests.Extensions
{
    public static class ActionResultExtensions
    {
        public static ApiResponse ToApiResponse(this IActionResult actionResult)
        {
            var objectResult = (ObjectResult)actionResult;
            return (ApiResponse)objectResult.Value;
        }

        public static ApiResponse<T> ToApiResponse<T>(this IActionResult actionResult)
        {
            var objectResult = (ObjectResult)actionResult;
            return (ApiResponse<T>)objectResult.Value;
        }
    }
}
