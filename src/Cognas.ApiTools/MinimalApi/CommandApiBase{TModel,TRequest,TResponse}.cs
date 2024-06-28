using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Mapping;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Net.Mime;

namespace Cognas.ApiTools.MinimalApi;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class CommandApiBase<TModel, TRequest, TResponse> : ICommandApi<TModel, TRequest, TResponse>
    where TModel : class
    where TRequest : notnull
    where TResponse : class
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public abstract int ApiVersion { get; }

    /// <summary>
    /// 
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// 
    /// </summary>
    public string PluralModelName { get; }

    /// <summary>
    /// 
    /// </summary>
    public string LowerPluralModelName { get; }

    /// <summary>
    /// 
    /// </summary>
    public ICommandMappingService<TModel, TRequest, TResponse> CommandMappingService { get; }

    /// <summary>
    /// 
    /// </summary>
    public IQueryMappingService<TModel, TResponse> QueryMappingService { get; }

    /// <summary>
    /// 
    /// </summary>
    public IModelIdService ModelIdService { get; }

    /// <summary>
    /// 
    /// </summary>
    public ICommandBusinessLogic<TModel> CommandBusinessLogic { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="CommandApiBase{TModel,TRequest,TResponse}"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="commandMappingService"></param>
    /// <param name="queryMappingService"></param>
    /// <param name="modelIdService"></param>
    /// <param name="commandBusinessLogic"></param>
    protected CommandApiBase(ILogger logger,
                             ICommandMappingService<TModel, TRequest, TResponse> commandMappingService,
                             IQueryMappingService<TModel, TResponse> queryMappingService,
                             IModelIdService modelIdService,
                             ICommandBusinessLogic<TModel> commandBusinessLogic)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(commandMappingService, nameof(commandMappingService));
        ArgumentNullException.ThrowIfNull(queryMappingService, nameof(queryMappingService));
        ArgumentNullException.ThrowIfNull(modelIdService, nameof(modelIdService));
        ArgumentNullException.ThrowIfNull(commandBusinessLogic, nameof(commandBusinessLogic));

        Logger = logger;
        CommandMappingService = commandMappingService;
        QueryMappingService = queryMappingService;
        ModelIdService = modelIdService;
        CommandBusinessLogic = commandBusinessLogic;

        PluralModelName = PluralsService.Instance.PluraliseModelName<TModel>();
        LowerPluralModelName = PluralModelName.ToLowerInvariant();
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual void MapAll(IEndpointRouteBuilder endpointRouteBuilder)
    {
        MapPost(endpointRouteBuilder);
        MapPut(endpointRouteBuilder);
        MapDelete(endpointRouteBuilder);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual RouteHandlerBuilder MapPost(IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapPost
        (
            $"/{LowerPluralModelName}",
            async
            (
                HttpContext httpContext,
                [FromBody] TRequest request
            ) =>
            {
                return await PostAsync(httpContext, request).ConfigureAwait(false);
            }
        )
        .MapToApiVersion(ApiVersion)
        .WithName($"Post{PluralModelName}V{ApiVersion}")
        .WithTags(PluralModelName)
        .WithOpenApi(configureOperation => new(configureOperation)
        {
            Summary = $"Posts a new model via the '{typeof(TRequest).Name}' request.",
            Tags = [new() { Name = PluralModelName }]
        })
        .Accepts<TRequest>(MediaTypeNames.Application.Json)
        .Produces<TResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual RouteHandlerBuilder MapPut(IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapPut
        (
            $"/{LowerPluralModelName}/{{id}}",
            async
            (
                [FromRoute] int id,
                [FromBody] TRequest request
            ) =>
            {
                return await PutAsync(id, request).ConfigureAwait(false);
            }
        )
        .MapToApiVersion(ApiVersion)
        .WithName($"Put{PluralModelName}V{ApiVersion}")
        .WithTags(PluralModelName)
        .WithOpenApi(configureOperation => new(configureOperation)
        {
            Summary = $"Puts an existing model via the '{typeof(TRequest).Name}' request. The required '{configureOperation.Parameters[0].Name}' parameter should match the {configureOperation.Parameters[0].Name} in the request.",
            Tags = [new() { Name = PluralModelName }]
        })
        .Accepts<TRequest>(MediaTypeNames.Application.Json)
        .Produces<TResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual RouteHandlerBuilder MapDelete(IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapDelete
        (
            $"/{LowerPluralModelName}/{{id}}",
            async
            (
                [FromRoute] int id
            ) =>
            {
                return await DeleteAsync(id).ConfigureAwait(false);
            }
        )
        .MapToApiVersion(ApiVersion)
        .WithName($"Delete{PluralModelName}V{ApiVersion}")
        .WithTags(PluralModelName)
        .WithOpenApi(configureOperation => new(configureOperation)
        {
            Summary = $"Deletes a single model via the required '{configureOperation.Parameters[0].Name}' parameter.",
            Tags = [new() { Name = PluralModelName }]
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    #endregion

    #region Private Method Declaration

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<Results<Created<TResponse>, BadRequest>> PostAsync(HttpContext httpContext, TRequest request)
    {
        TModel model = CommandMappingService.RequestToModel(request);
        TModel? insertedModel = await CommandBusinessLogic.InsertModelAsync(model).ConfigureAwait(false);
        if (insertedModel != null)
        {
            int id = ModelIdService.GetId(insertedModel);
            TResponse response = QueryMappingService.ModelToResponse(insertedModel);
            string uri = BuildNewModelUri(httpContext, id);
            return TypedResults.Created(uri, response);
        }
        return TypedResults.BadRequest();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<Results<Ok<TResponse>, BadRequest>> PutAsync(int id, TRequest request)
    {
        TModel model = CommandMappingService.RequestToModel(request);
        if (ModelIdService.GetId(model) != id)
        {
            return TypedResults.BadRequest();
        }
        TModel? updatedModel = await CommandBusinessLogic.UpdateModelAsync(model).ConfigureAwait(false);
        if (updatedModel != null)
        {
            TResponse response = QueryMappingService.ModelToResponse(updatedModel);
            return TypedResults.Ok(response);
        }
        return TypedResults.BadRequest();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<Results<Ok, NotFound>> DeleteAsync(int id)
    {
        IParameter parameter = ModelIdService.IdParameter<TModel>(id);
        bool success = await CommandBusinessLogic.DeleteModelAsync(parameter).ConfigureAwait(false);
        if (success)
        {
            return TypedResults.Ok();
        }
        return TypedResults.NotFound();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private string BuildNewModelUri(HttpContext httpContext, int id)
    {
        string newModelUri = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{LowerPluralModelName}/{id}";
        return newModelUri;
    }

    #endregion
}