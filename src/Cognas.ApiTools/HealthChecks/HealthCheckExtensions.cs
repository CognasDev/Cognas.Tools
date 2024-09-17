using Microsoft.Extensions.DependencyInjection;

namespace Cognas.ApiTools.HealthChecks;

/// <summary>
/// 
/// </summary>
public static class HealthCheckExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddDefaultHealthChecks(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IHealthCheckResultHelper, HealthCheckResultHelper>();
        IHealthChecksBuilder healthChecksBuilder = serviceCollection.AddHealthChecks()
                                                                    .AddCheck<ApiHealthCheck>(nameof(ApiHealthCheck))
                                                                    .AddCheck<DatabaseHealthCheck>(nameof(DatabaseHealthCheck));
        return healthChecksBuilder;
    }

    #endregion
}