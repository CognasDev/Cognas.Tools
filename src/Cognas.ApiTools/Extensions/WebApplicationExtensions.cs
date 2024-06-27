using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;
using Cognas.ApiTools.Shared.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

namespace Cognas.ApiTools.Extensions;

/// <summary>
/// 
/// </summary>
public static class WebApplicationExtensions
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
    /// <param name="webApplication"></param>
    /// <param name="healthCheckEndpoint"></param>
    public static void ConfigureAndRun(this WebApplication webApplication, string healthCheckEndpoint = "/health")
    {
        webApplication.UseAuthorization();
        webApplication.UseExceptionHandler();
        webApplication.UseHttpsRedirection();
        webApplication.MapHealthChecks(healthCheckEndpoint, new()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        webApplication.Run();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    /// <param name="majorVersion"></param>
    /// <returns></returns>
    public static RouteGroupBuilder GetApiVersionRoute(this WebApplication webApplication, int majorVersion)
    {
        ApiVersionSet apiVersionSet = webApplication.NewApiVersionSet().HasApiVersion(majorVersion).ReportApiVersions().Build();
        RouteGroupBuilder routeGroupBuilder = webApplication.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(apiVersionSet);
        return routeGroupBuilder;
    }

    #endregion
}