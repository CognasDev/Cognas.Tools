using Serilog;

namespace Cognas.ApiTools.Logging;

/// <summary>
/// 
/// </summary>
public static class FileLogging
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerConfiguration"></param>
    /// <param name="path"></param>
    public static void ConfigureFileLogging(this LoggerConfiguration loggerConfiguration, string path)
    {
        loggerConfiguration.WriteTo.File(path, rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                           .Enrich.FromLogContext();
    }

    #endregion
}