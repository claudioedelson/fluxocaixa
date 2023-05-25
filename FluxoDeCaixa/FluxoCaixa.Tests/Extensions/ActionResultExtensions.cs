using FluxoDeCaixa.API.Models;
using Microsoft.AspNetCore.Mvc;



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
