using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Cognas.ApiTools.Logging;

/// <summary>
/// 
/// </summary>
public static class ApplicationInsightsLogging
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void ConfigureApplicationInsightsLogging(this WebApplicationBuilder webApplicationBuilder)
    {
        string connectionString = webApplicationBuilder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString") ?? throw new NullReferenceException("ApplicationInsights");
        TelemetryConfiguration telemetryConfiguration = new() { ConnectionString = connectionString };
        webApplicationBuilder.Logging.AddAzureWebAppDiagnostics();
        webApplicationBuilder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration)
                                                                                                                       .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces));
    }
}