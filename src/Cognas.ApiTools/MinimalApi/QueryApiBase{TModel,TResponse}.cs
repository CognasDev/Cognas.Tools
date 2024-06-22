using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Mapping;
using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Cognas.ApiTools.MinimalApi;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class QueryApiBase<TModel, TResponse> : IQueryApi<TModel, TResponse> where TModel : class where TResponse : class
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
    public IQueryMappingService<TModel, TResponse> MappingService { get; }

    /// <summary>
    /// 
    /// </summary>
    public IModelIdService ModelIdService { get; }

    /// <summary>
    /// 
    /// </summary>
    public IPaginationFunctions PaginationFunctions { get; }

    /// <summary>
    /// 
    /// </summary>
    public IQueryBusinessLogic<TModel> QueryBusinessLogic { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="QueryApiBase{TModel,TResponse}"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mappingService"></param>
    /// <param name="modelIdService"></param>
    /// <param name="paginationFunctions"></param>
    /// <param name="queryBusinessLogic"></param>
    protected QueryApiBase(ILogger logger,
                           IQueryMappingService<TModel, TResponse> mappingService,
                           IModelIdService modelIdService,
                           IPaginationFunctions paginationFunctions,
                           IQueryBusinessLogic<TModel> queryBusinessLogic)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(mappingService, nameof(mappingService));
        ArgumentNullException.ThrowIfNull(modelIdService, nameof(modelIdService));
        ArgumentNullException.ThrowIfNull(paginationFunctions, nameof(paginationFunctions));
        ArgumentNullException.ThrowIfNull(queryBusinessLogic, nameof(queryBusinessLogic));

        Logger = logger;
        MappingService = mappingService;
        ModelIdService = modelIdService;
        PaginationFunctions = paginationFunctions;
        QueryBusinessLogic = queryBusinessLogic;

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
        MapGet(webApplication);
        MapGetById(webApplication);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    public virtual void MapGet(WebApplication webApplication)
    {
        webApplication.MapGet
        (
            $"/{LowerPluralModelName}",
            (
                CancellationToken cancellationToken,
                HttpContext httpContext,
                [FromQuery][Range(1, int.MaxValue)] int? pageSize = null,
                [FromQuery][Range(1, int.MaxValue)] int? pageNumber = null,
                [FromQuery] string? orderBy = null,
                [FromQuery] bool? orderByAscending = null

            ) =>
            {
                PaginationQuery paginationQuery = new()
                {
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    OrderBy = orderBy,
                    OrderByAscending = orderByAscending ?? true
                };
                return Get(httpContext, paginationQuery, cancellationToken);
            }
        )
        .WithName($"Get{PluralModelName}")
        .WithTags(PluralModelName)
        .WithOpenApi();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    public virtual void MapGetById(WebApplication webApplication)
    {
        webApplication.MapGet
        (
            $"/{LowerPluralModelName}/{{id}}",
            async
            (
                [FromRoute] int id
            ) =>
            {
                return await GetAsync(id).ConfigureAwait(false);
            }
        )
        .WithName($"Get{PluralModelName}ById")
        .WithTags(PluralModelName)
        .WithOpenApi();
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="paginationQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="PaginationQueryParametersException"></exception>
    private async IAsyncEnumerable<TResponse> Get(HttpContext httpContext, PaginationQuery paginationQuery, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IEnumerable<TResponse> responses = PaginationFunctions.IsPaginationQueryValidOrDefault<TResponse>(paginationQuery) switch
        {
            false => throw new PaginationQueryParametersException(paginationQuery),
            true => await GetResponsesWithPaginationAsync(httpContext, paginationQuery).ConfigureAwait(false),
            _ => await GetResponsesAsync().ConfigureAwait(false),
        };

        foreach (TResponse response in responses)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return response;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<Results<Ok<TResponse>, NotFound>> GetAsync(int id)
    {
        TModel? model = await QueryBusinessLogic.SelectModelAsync(id, ModelIdService.IdParameter<TModel>(id)).ConfigureAwait(false);
        return TryMapModelToDto(model);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    private Results<Ok<TResponse>, NotFound> TryMapModelToDto(TModel? model)
    {
        if (model != null)
        {
            TResponse response = MappingService.ModelToResponse(model);
            return TypedResults.Ok(response);
        }
        return TypedResults.NotFound();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private async Task<IEnumerable<TResponse>> GetResponsesAsync()
    {
        IEnumerable<TModel> models = await QueryBusinessLogic.SelectModelsAsync().ConfigureAwait(false);
        IEnumerable<TResponse> responses = MappingService.ModelsToResponses(models);
        return responses;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="paginationQuery"></param>
    /// <returns></returns>
    private async Task<IEnumerable<TResponse>> GetResponsesWithPaginationAsync(HttpContext httpContext, IPaginationQuery paginationQuery)
    {
        IEnumerable<TModel> models = await QueryBusinessLogic.SelectModelsAsync().ConfigureAwait(false);
        IEnumerable<TResponse> responses = MappingService.ModelsToResponses(models);
        PropertyDescriptor orderByProperty = PaginationFunctions.OrderByProperty<TResponse>(paginationQuery);
        int takeQuantity = PaginationFunctions.TakeQuantity(paginationQuery);
        int skipNumber = PaginationFunctions.SkipNumber(paginationQuery);

        if (paginationQuery.OrderByAscending)
        {
            responses = responses.OrderBy(model => orderByProperty.GetValue(model)).Skip(skipNumber).Take(takeQuantity);
        }
        else
        {
            responses = responses.OrderByDescending(model => orderByProperty.GetValue(model)).Skip(skipNumber).Take(takeQuantity);
        }

        PaginationFunctions.BuildPaginationResponseHeader(paginationQuery, models, httpContext);
        return responses;
    }

    #endregion
}