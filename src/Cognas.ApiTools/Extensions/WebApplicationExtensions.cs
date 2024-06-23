﻿using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;
using Cognas.ApiTools.MinimalApi;
using HealthChecks.UI.Client;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
    public static void AddSwagger(this WebApplication webApplication)
    {
        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    /// <param name="healthCheckEndpoint"></param>
    public static void ConfigureAndRun(this WebApplication webApplication, string healthCheckEndpoint = "/health")
    {
        webApplication.UseAuthorization();
        webApplication.UseExceptionHandler();
        webApplication.UseHttpsRedirection();
        webApplication.MapHealthChecks(healthCheckEndpoint, new()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        webApplication.Run();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="webApplication"></param>
    /// <param name="endpointRouteBuilder"></param>
    public static void InitiateApi<TModel, TResponse>(this WebApplication webApplication, IEndpointRouteBuilder? endpointRouteBuilder = null)
        where TModel : class
        where TResponse : class
    {
        IQueryApi<TModel, TResponse> queryApi = webApplication.Services.GetQueryApi<TModel, TResponse>();
        queryApi.MapAll(endpointRouteBuilder ?? webApplication);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="webApplication"></param>
    /// <param name="endpointRouteBuilder"></param>
    public static void InitiateApi<TModel, TRequest, TResponse>(this WebApplication webApplication, IEndpointRouteBuilder? endpointRouteBuilder = null)
        where TModel : class
        where TRequest : class
        where TResponse : class
    {
        ICommandApi<TModel, TRequest, TResponse> commandApi = webApplication.Services.GetCommandApi<TModel, TRequest, TResponse>();
        commandApi.MapAll(endpointRouteBuilder ?? webApplication);
        webApplication.InitiateApi<TModel, TResponse>(endpointRouteBuilder);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    /// <param name="majorVersion"></param>
    /// <param name="minorVersion"></param>
    /// <returns></returns>
    public static RouteGroupBuilder GetApiVersionRoute(this WebApplication webApplication, int majorVersion, int? minorVersion = null)
    {
        ApiVersionSet apiVersionSet = webApplication.NewApiVersionSet().HasApiVersion(majorVersion, minorVersion).ReportApiVersions().Build();
        RouteGroupBuilder routeGroupBuilder = webApplication.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(apiVersionSet);
        return routeGroupBuilder;
    }

    #endregion
}