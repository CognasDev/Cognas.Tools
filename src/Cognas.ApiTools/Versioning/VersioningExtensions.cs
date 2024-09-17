using Asp.Versioning;
using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Cognas.ApiTools.Versioning;

/// <summary>
/// 
/// </summary>
public static class VersioningExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="defaulApiVersion"></param>
    public static void AddVersioning(this IServiceCollection serviceCollection, int defaulApiVersion = 1)
    {
        serviceCollection.AddApiVersioning(apiVersioningAction =>
        {
            const string xApiVersion = "x-api-version";
            apiVersioningAction.DefaultApiVersion = new ApiVersion(defaulApiVersion);
            apiVersioningAction.AssumeDefaultVersionWhenUnspecified = true;
            apiVersioningAction.ReportApiVersions = true;
            apiVersioningAction.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                                            new HeaderApiVersionReader(xApiVersion),
                                                                            new MediaTypeApiVersionReader(xApiVersion));
        })
        .AddApiExplorer(apiExplorerOptions =>
        {
            apiExplorerOptions.GroupNameFormat = "'v'V";
            apiExplorerOptions.SubstituteApiVersionInUrl = true;
        });
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