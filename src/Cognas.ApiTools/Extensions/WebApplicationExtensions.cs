using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;

namespace Cognas.ApiTools.Extensions;

/// <summary>
/// 
/// </summary>
public static class WebApplicationExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    /// <param name="healthCheckEndpoint"></param>
    public static void ConfigureAndRun(this WebApplication webApplication, string healthCheckEndpoint = "/health")
    {
        webApplication.UseAuthorization();
        webApplication.UseExceptionHandler();
        //webApplication.UseHttpsRedirection();
        webApplication.MapHealthChecks(healthCheckEndpoint, new()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        webApplication.Run();
    }

    #endregion
}