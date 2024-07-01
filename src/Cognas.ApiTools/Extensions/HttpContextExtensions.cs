using Microsoft.AspNetCore.Http;

namespace Cognas.ApiTools.Extensions;

/// <summary>
/// 
/// </summary>
public static class HttpContextExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="route"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string BuildLocationUri(this HttpContext httpContext, string route, int id)
    {
        string locationUri = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{route}/{id}";
        return locationUri;
    }

    #endregion
}