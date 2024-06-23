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

namespace Cognas.ApiTools.MinimalApi;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class CommandApiBase<TModel, TRequest, TResponse> : ICommandApi<TModel, TRequest, TResponse>
    where TModel : class
    where TRequest : class
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
    public virtual void MapPost(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost
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
        .WithOpenApi();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual void MapPut(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPut
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
        .WithOpenApi();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual void MapDelete(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapDelete
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
        .WithOpenApi();
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