using Microsoft.Extensions.Logging;
using Serilog;

namespace Cognas.ApiTools.Extensions;

/// <summary>
/// 
/// </summary>
public static class LoggingBuilderExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggingBuilder"></param>
    /// <param name="path"></param>
    public static void ConfigureLocalLogging(this ILoggingBuilder loggingBuilder, string path = "log-.log")
    {
        Serilog.ILogger logger = new LoggerConfiguration()
                                    .WriteTo.File(path, rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                                    .Enrich.FromLogContext().CreateLogger();
        loggingBuilder.ClearProviders();
        loggingBuilder.AddSerilog(logger);
    }

    #endregion
}