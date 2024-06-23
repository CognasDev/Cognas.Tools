using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Cognas.ApiTools.Extensions;

/// <summary>
/// 
/// </summary>
public static class WebApplicationBuilderExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TBind"></typeparam>
    /// <param name="webApplicationBuilder"></param>
    public static void BindConfigurationSection<TBind>(this WebApplicationBuilder webApplicationBuilder) where TBind : class
        => webApplicationBuilder.Services.Configure<TBind>(webApplicationBuilder.Configuration.GetSection(typeof(TBind).Name));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void ConfigureApplicationInsightsLogging(this WebApplicationBuilder webApplicationBuilder)
    {
        string connectionString = webApplicationBuilder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString") ?? throw new NullReferenceException("ApplicationInsights");
        TelemetryConfiguration telemetryConfiguration = new() { ConnectionString = connectionString };
        webApplicationBuilder.Logging.ClearProviders();
        webApplicationBuilder.Logging.AddAzureWebAppDiagnostics();
        webApplicationBuilder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration)
                                                                                                                       .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces));
    }

    #endregion
}