using Asp.Versioning.ApiExplorer;
using Cognas.ApiTools.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Cognas.ApiTools.Services;

/// <summary>
/// 
/// </summary>
public sealed class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    #region Field Declaration

    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ConfigureSwaggerGenOptions"/>
    /// </summary>
    /// <param name="apiVersionDescriptionProvider"></param>
    public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        ArgumentNullException.ThrowIfNull(apiVersionDescriptionProvider, nameof(apiVersionDescriptionProvider));
        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }

    #endregion

    #region Public Method Declarations
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    public void Configure(string? name, SwaggerGenOptions options) => Configure(options);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Configure(SwaggerGenOptions options)
    {
        _apiVersionDescriptionProvider.ApiVersionDescriptions.FastForEach(apiVersionDescription =>
        {
            OpenApiInfo openApiInfo = new()
            {
                Title = Assembly.GetEntryAssembly()!.GetName().Name,
                Version = apiVersionDescription.ApiVersion.ToString()
            };
            options.SwaggerDoc(apiVersionDescription.GroupName, openApiInfo);
        });
    }

    #endregion
}