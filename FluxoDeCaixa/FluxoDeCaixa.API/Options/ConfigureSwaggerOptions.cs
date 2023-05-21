using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FluxoDeCaixa.API.Options
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            // Add a swagger document for each discovered API version.
            // NOTE: you might choose to skip or document deprecated API versions differently.
            foreach (var description in _provider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var openApiInfo = new OpenApiInfo
            {
                Title = "Controle de Fluxo Caixa (CFC)",
                Description = "ASP.NET Core C# REST API, DDD, Princípios SOLID e Clean Architecture",
                Version = description.ApiVersion.ToString(),
                Contact = new OpenApiContact
                {
                    Name = "Claudio Mesquita",
                    Email = "claudio.edelson@gmail.com",
#pragma warning disable S1075 // Refactor your code not to use hardcoded absolute paths or URIs.
                    Url = new Uri("https://www.linkedin.com/in/claudio-mesquita-a0a56331/")
#pragma warning restore S1075 // Refactor your code not to use hardcoded absolute paths or URIs.
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
#pragma warning disable S1075 // Refactor your code not to use hardcoded absolute paths or URIs.
                    Url = new Uri("https://github.com/claudioedelson/fluxocaixa/blob/main/LICENSE")
#pragma warning restore S1075 // Refactor your code not to use hardcoded absolute paths or URIs.
                }
            };

            if (description.IsDeprecated)
                openApiInfo.Description += " - Esta versão da API foi descontinuada.";

            return openApiInfo;
        }
    }
}
