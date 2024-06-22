using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Cognas.ApiTools.HealthChecks;

/// <summary>
/// 
/// </summary>
public sealed class ApiHealthCheck : IHealthCheck
{
    #region Field Declarations

    private readonly IHealthCheckResultHelper _healthCheckResultHelper;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ApiHealthCheck"/>
    /// </summary>
    /// <param name="healthCheckResultHelper"></param>
    public ApiHealthCheck(IHealthCheckResultHelper healthCheckResultHelper)
    {
        ArgumentNullException.ThrowIfNull(healthCheckResultHelper, nameof(healthCheckResultHelper));
        _healthCheckResultHelper = healthCheckResultHelper;
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            HealthCheckResult healthyResult = _healthCheckResultHelper.Healthy("Api is running.");
            return await Task.FromResult(healthyResult).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            HealthCheckResult apiDownResult = _healthCheckResultHelper.Failed(context.Registration.FailureStatus, "Api is down.", exception);
            return await Task.FromResult(apiDownResult).ConfigureAwait(false);
        }
    }

    #endregion
}