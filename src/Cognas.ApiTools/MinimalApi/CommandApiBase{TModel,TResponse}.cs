using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Mapping;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public ICommandMappingService<TModel, TRequest, TResponse> MappingService { get; }

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
    /// <param name="mappingService"></param>
    /// <param name="modelIdService"></param>
    /// <param name="commandBusinessLogic"></param>
    protected CommandApiBase(ILogger logger,
                             ICommandMappingService<TModel, TRequest, TResponse> mappingService,
                             IModelIdService modelIdService,
                             ICommandBusinessLogic<TModel> commandBusinessLogic)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(mappingService, nameof(mappingService));
        ArgumentNullException.ThrowIfNull(modelIdService, nameof(modelIdService));
        ArgumentNullException.ThrowIfNull(commandBusinessLogic, nameof(commandBusinessLogic));

        Logger = logger;
        MappingService = mappingService;
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
    /// <param name="webApplication"></param>
    public virtual void MapAll(WebApplication webApplication)
    {
        MapPost(webApplication);
        MapPut(webApplication);
        MapDelete(webApplication);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    public virtual void MapPost(WebApplication webApplication)
    {
        webApplication.MapPost
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
        .WithName($"Post{PluralModelName}")
        .WithTags(PluralModelName)
        .WithOpenApi();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    public virtual void MapPut(WebApplication webApplication)
    {
        webApplication.MapPut
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
        .WithName($"Put{PluralModelName}")
        .WithTags(PluralModelName)
        .WithOpenApi();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    public virtual void MapDelete(WebApplication webApplication)
    {
        webApplication.MapDelete
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
        .WithName($"Delete{PluralModelName}")
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
        TModel model = MappingService.RequestToModel(request);
        TModel? insertedModel = await CommandBusinessLogic.InsertModelAsync(model).ConfigureAwait(false);
        if (insertedModel != null)
        {
            int id = ModelIdService.GetId(insertedModel);
            TResponse response = MappingService.ModelToResponse(insertedModel);
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
        TModel model = MappingService.RequestToModel(request);
        if (ModelIdService.GetId(model) != id)
        {
            return TypedResults.BadRequest();
        }
        TModel? updatedModel = await CommandBusinessLogic.UpdateModelAsync(model).ConfigureAwait(false);
        if (updatedModel != null)
        {
            TResponse response = MappingService.ModelToResponse(updatedModel);
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