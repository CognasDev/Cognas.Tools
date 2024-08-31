namespace Cognas.ApiTools.Logging;

/// <summary>
/// 
/// </summary>
/// <param name="loggingType"></param>
public sealed class LoggingConfigurationExecption(LoggingType loggingType) : Exception($"Incorrect configuration for {loggingType}.")
{
}