﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Cognas.ApiTools.Logging;

/// <summary>
/// 
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <param name="loggingType"></param>
    /// <param name="fileLoggingPath"></param>
    /// <param name="openTelemetryEndPoint"></param>
    /// <param name="openTelemetryApiKeyHeader"></param>
    /// <param name="openTelemetryApiKeyValue"></param>
    /// <param name="openTelemetryServiceName"></param>
    /// <exception cref="LoggingConfigurationExecption"></exception>
    public static void ConfigureLogging(this WebApplicationBuilder webApplicationBuilder, LoggingType loggingType,
                                        string? fileLoggingPath = "log-.log",
                                        string? openTelemetryEndPoint = null,
                                        string? openTelemetryApiKeyHeader = "X-Seq-ApiKey",
                                        string? openTelemetryApiKeyValue = null,
                                        string? openTelemetryServiceName = null)
    {
        LoggerConfiguration loggerConfiguration = new();
        if (loggingType.HasFlag(LoggingType.File))
        {
            if (string.IsNullOrWhiteSpace(fileLoggingPath))
            {
                throw new LoggingConfigurationExecption(LoggingType.File);
            }
            loggerConfiguration.ConfigureFileLogging(fileLoggingPath);
        }
        if (loggingType.HasFlag(LoggingType.ApplicationInsights))
        {
            webApplicationBuilder.ConfigureApplicationInsightsLogging();
            webApplicationBuilder.Services.AddApplicationInsightsTelemetry();
        }
        if (loggingType.HasFlag(LoggingType.OpenTelemetry))
        {
            if (string.IsNullOrWhiteSpace(openTelemetryEndPoint) ||
                string.IsNullOrWhiteSpace(openTelemetryApiKeyHeader) ||
                string.IsNullOrWhiteSpace(openTelemetryApiKeyValue) ||
                string.IsNullOrWhiteSpace(openTelemetryServiceName))
            {
                throw new LoggingConfigurationExecption(LoggingType.OpenTelemetry);
            }
            loggerConfiguration.ConfigureOpenTelemetryLogging(openTelemetryEndPoint, openTelemetryApiKeyHeader, openTelemetryApiKeyValue, openTelemetryServiceName);
        }
        ILogger logger = loggerConfiguration.CreateLogger();
        webApplicationBuilder.Logging.ClearProviders();
        webApplicationBuilder.Logging.AddSerilog(logger);
    }
}