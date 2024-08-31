namespace Cognas.ApiTools.Logging;

/// <summary>
/// 
/// </summary>
[Flags]
#pragma warning disable 1591
public enum LoggingType
{
    File = 0,
    ApplicationInsights = 1,
    OpenTelemetry = 2
}
#pragma warning restore 1591