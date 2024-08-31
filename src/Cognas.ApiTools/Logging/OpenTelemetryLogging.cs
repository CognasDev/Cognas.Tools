using Serilog;

namespace Cognas.ApiTools.Logging;

/// <summary>
/// 
/// </summary>
public static class OpenTelemetryLogging
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="endPoint"></param>
    /// <param name="apiKeyHeader"></param>
    /// <param name="apiKeyValue"></param>
    /// <param name="serviceName"></param>
    public static void ConfigureOpenTelemetryLogging(this LoggerConfiguration logger, string endPoint, string apiKeyHeader, string apiKeyValue, string serviceName)
    {
        logger.WriteTo.OpenTelemetry(sinkOptions =>
        {
            sinkOptions.Endpoint = endPoint;
            sinkOptions.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.HttpProtobuf;
            sinkOptions.Headers = new Dictionary<string, string>(1) { [apiKeyHeader] = apiKeyValue };
            sinkOptions.ResourceAttributes = new Dictionary<string, object>(1) { ["service.name"] = serviceName };
        });
    }

    #endregion
}