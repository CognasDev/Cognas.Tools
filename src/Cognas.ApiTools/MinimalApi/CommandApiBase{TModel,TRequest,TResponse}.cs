using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.Mapping;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.Shared.Services;
using LanguageExt.Common;
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
    protected ILogger Logger { get; }

    /// <summary>
    /// 
    /// </summary>
    protected string PluralModelName { get; }

    /// <summary>
    /// 
    /// </summary>
    protected string LowerPluralModelName { get; }

    /// <summary>
    /// 
    /// </summary>
    protected ICommandMappingService<TModel, TRequest> CommandMappingService { get; }

    /// <summary>
    /// 
    /// </summary>
    protected IQueryMappingService<TModel, TResponse> QueryMappingService { get; }

    /// <summary>
    /// 
    /// </summary>
    protected IModelIdService ModelIdService { get; }

    /// <summary>
    /// 
    /// </summary>
    protected ICommandBusinessLogic<TModel> CommandBusinessLogic { get; }

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
                             ICommandMappingService<TModel, TRequest> commandMappingService,
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
    private async Task<Results<CreatedAtRoute<TResponse>, BadRequest>> PostAsync(HttpContext httpContext, TRequest request)
    {
        TModel model = CommandMappingService.RequestToModel(request);
        Result<TModel> insertResult = await CommandBusinessLogic.InsertModelAsync(model).ConfigureAwait(false);
        Results<CreatedAtRoute<TResponse>, BadRequest> apiResult = insertResult.Match<Results<CreatedAtRoute<TResponse>, BadRequest>>
        (
            insertedModel =>
            {
                int id = ModelIdService!.GetId(insertedModel);
                TResponse response = QueryMappingService.ModelToResponse(insertedModel);
                string routeName = $"Post{PluralModelName}V{ApiVersion}";
                return TypedResults.CreatedAtRoute<TResponse>(response, routeName, id);
            },
            exception => TypedResults.BadRequest()
        );
        return apiResult;
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
        Result<TModel> updateResult = await CommandBusinessLogic.UpdateModelAsync(model).ConfigureAwait(false);
        Results<Ok<TResponse>, BadRequest> apiResult = updateResult.Match<Results<Ok<TResponse>, BadRequest>>
        (
            updatedModel =>
            {
                TResponse response = QueryMappingService.ModelToResponse(updatedModel);
                return TypedResults.Ok(response);
            },
            exception => TypedResults.BadRequest()
        );
        return apiResult;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<Results<Ok, NotFound>> DeleteAsync(int id)
    {
        IParameter idParameter = ModelIdService.IdParameter<TModel>(id);
        Result<bool> deleteResult = await CommandBusinessLogic.DeleteModelAsync(idParameter).ConfigureAwait(false);
        Results<Ok, NotFound> apiResult = deleteResult.Match<Results<Ok, NotFound>>
        (
            success => TypedResults.Ok(),
            exception => TypedResults.NotFound()
        );
        return apiResult;
    }

    #endregion
}