using Microsoft.AspNetCore.Mvc;
using Samples.MusicCollection.Api.Albums;
using System.Net.Mime;

namespace Samples.MusicCollection.Api.AllMusic.Extensions;

/// <summary>
/// 
/// </summary>
internal static class RouteHandlerBuilderExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="routeHandlerBuilder"></param>
    /// <param name="apiVersion"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static RouteHandlerBuilder MapGetConfiguration<TResponse>(this RouteHandlerBuilder routeHandlerBuilder, int apiVersion, string tag)
    {
        return routeHandlerBuilder
           .MapToApiVersion(apiVersion)
           .WithTags(tag)
           .WithOpenApi()
           .Produces<IEnumerable<TResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
           .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
           .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="routeHandlerBuilder"></param>
    /// <param name="apiVersion"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static RouteHandlerBuilder MapGetByIdConfiguration<TResponse>(this RouteHandlerBuilder routeHandlerBuilder, int apiVersion, string tag)
    {
        return routeHandlerBuilder
           .MapToApiVersion(apiVersion)
           .WithTags(tag)
           .WithOpenApi()
           .Produces<TResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
           .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
           .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="routeHandlerBuilder"></param>
    /// <param name="apiVersion"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static RouteHandlerBuilder MapPostConfiguration<TRequest, TResponse>(this RouteHandlerBuilder routeHandlerBuilder, int apiVersion, string tag)
        where TRequest : notnull
    {
        return routeHandlerBuilder
            .MapToApiVersion(apiVersion)
            .WithTags(tag)
            .WithOpenApi()
            .Accepts<TRequest>(MediaTypeNames.Application.Json)
            .Produces<TResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="routeHandlerBuilder"></param>
    /// <param name="apiVersion"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static RouteHandlerBuilder MapPutConfiguration<TRequest, TResponse>(this RouteHandlerBuilder routeHandlerBuilder, int apiVersion, string tag)
        where TRequest : notnull
    {
        return routeHandlerBuilder
            .MapToApiVersion(apiVersion)
            .WithTags(tag)
            .WithOpenApi()
            .Accepts<AlbumRequest>(MediaTypeNames.Application.Json)
            .Produces<AlbumResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="routeHandlerBuilder"></param>
    /// <param name="apiVersion"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static RouteHandlerBuilder MapDeleteConfiguration(this RouteHandlerBuilder routeHandlerBuilder, int apiVersion, string tag)
    {
        return routeHandlerBuilder
            .MapToApiVersion(apiVersion)
            .WithTags(tag)
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    #endregion
}