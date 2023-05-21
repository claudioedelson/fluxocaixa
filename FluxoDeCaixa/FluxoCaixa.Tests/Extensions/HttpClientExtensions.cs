using Ardalis.GuardClauses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluxoDeCaixa.Shared.Extensions;

namespace FluxoCaixa.Tests.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TResponse> GetAsync<TResponse>(
            this HttpClient httpClient,
            string endpoint)
        {
            Guard.Against.Null(httpClient, nameof(httpClient));
            Guard.Against.NullOrWhiteSpace(endpoint, nameof(endpoint));

            using var httpResponse = await httpClient.GetAsync(endpoint);
            return await ConvertResponseToTypeAsync<TResponse>(httpResponse);
        }

        public static async Task<TResponse> PostAsync<TResponse>(
            this HttpClient httpClient,
            string endpoint,
            HttpContent httpContent)
        {
            Guard.Against.Null(httpClient, nameof(httpClient));
            Guard.Against.NullOrWhiteSpace(endpoint, nameof(endpoint));
            Guard.Against.Null(httpContent, nameof(httpContent));

            using var httpResponse = await httpClient.PostAsync(endpoint, httpContent);
            return await ConvertResponseToTypeAsync<TResponse>(httpResponse);
        }

        private static async Task<TResponse> ConvertResponseToTypeAsync<TResponse>(HttpResponseMessage httpResponse)
        {
            httpResponse.EnsureSuccessStatusCode(); // Status Code 200-299

            var response = await httpResponse.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(response);
            var jToken = jObject.SelectToken("result", errorWhenNoMatch: false);
            return jToken?.HasValues == true ? jToken.ToString().FromJson<TResponse>() : default;
        }
    }
}
