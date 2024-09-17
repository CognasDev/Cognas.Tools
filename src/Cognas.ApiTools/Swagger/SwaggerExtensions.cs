using Asp.Versioning.ApiExplorer;
using Cognas.Tools.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Cognas.ApiTools.Swagger;

/// <summary>
/// 
/// </summary>
public static class SwaggerExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    /// <param name="jsonFilename"></param>
    public static void AddSwagger(this WebApplication webApplication, string jsonFilename = "swagger")
    {
        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI(swaggerUiOptions =>
            {
                IReadOnlyList<ApiVersionDescription> apiVersionDescriptions = webApplication.DescribeApiVersions();
                apiVersionDescriptions.FastForEach(apiVersionDescription =>
                {
                    string url = $"/swagger/{apiVersionDescription.GroupName}/{jsonFilename}.json";
                    string name = apiVersionDescription.GroupName.ToUpperInvariant();
                    swaggerUiOptions.SwaggerEndpoint(url, name);
                });
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void ConfigureSwaggerGen(this IServiceCollection serviceCollection)
    {
        Assembly currentAssembly = Assembly.GetCallingAssembly();
        IEnumerable<string> xmlDocumentPaths = from assembly in currentAssembly.GetReferencedAssemblies().Union([currentAssembly.GetName()])
                                               let assemblyName = assembly.Name ?? throw new NullReferenceException(nameof(AssemblyName))
                                               let xmlDocumentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName}.xml")
                                               where File.Exists(xmlDocumentPath)
                                               select xmlDocumentPath;

        serviceCollection.AddSwaggerGen(swaggerGenAction =>
        {
            swaggerGenAction.DocumentFilter<SwaggerSortedDocumentFilter>();
            swaggerGenAction.DescribeAllParametersInCamelCase();
            xmlDocumentPaths.FastForEach(xmlDocumentPath => swaggerGenAction.IncludeXmlComments(xmlDocumentPath));
        });
    }

    #endregion
}