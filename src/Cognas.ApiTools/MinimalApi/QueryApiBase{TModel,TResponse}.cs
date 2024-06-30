using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Mapping;
using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.ComponentModel;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;

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
    protected IQueryMappingService<TModel, TResponse> QueryMappingService { get; }

    /// <summary>
    /// 
    /// </summary>
    protected IModelIdService ModelIdService { get; }

    /// <summary>
    /// 
    /// </summary>
    protected IPaginationFunctions PaginationFunctions { get; }

    /// <summary>
    /// 
    /// </summary>
    protected IQueryBusinessLogic<TModel> QueryBusinessLogic { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="QueryApiBase{TModel,TResponse}"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="queryMappingService"></param>
    /// <param name="modelIdService"></param>
    /// <param name="paginationFunctions"></param>
    /// <param name="queryBusinessLogic"></param>
    protected QueryApiBase(ILogger logger,
                           IQueryMappingService<TModel, TResponse> queryMappingService,
                           IModelIdService modelIdService,
                           IPaginationFunctions paginationFunctions,
                           IQueryBusinessLogic<TModel> queryBusinessLogic)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(queryMappingService, nameof(queryMappingService));
        ArgumentNullException.ThrowIfNull(modelIdService, nameof(modelIdService));
        ArgumentNullException.ThrowIfNull(paginationFunctions, nameof(paginationFunctions));
        ArgumentNullException.ThrowIfNull(queryBusinessLogic, nameof(queryBusinessLogic));

        Logger = logger;
        QueryMappingService = queryMappingService;
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
    /// <param name="endpointRouteBuilder"></param>
    public virtual void MapAll(IEndpointRouteBuilder endpointRouteBuilder)
    {
        MapGet(endpointRouteBuilder);
        MapGetById(endpointRouteBuilder);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual RouteHandlerBuilder MapGet(IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapGet
        (
            $"/{LowerPluralModelName}",
            (
                CancellationToken cancellationToken,
                HttpContext httpContext,
                [AsParameters] PaginationQuery paginationQuery
            ) =>
            {
                return Get(httpContext, paginationQuery, cancellationToken);
            }
        )
        .MapToApiVersion(ApiVersion)
        .WithName($"Get{PluralModelName}V{ApiVersion}")
        .WithTags(PluralModelName)
        .WithOpenApi(configureOperation => BuildGetAllOpenApi(configureOperation))
        .Produces<IEnumerable<TResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual RouteHandlerBuilder MapGetById(IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapGet
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
        .MapToApiVersion(ApiVersion)
        .WithName($"Get{PluralModelName}ByIdV{ApiVersion}")
        .WithTags(PluralModelName)
        .WithOpenApi(configureOperation => new(configureOperation)
        {
            Summary = $"Gets a single model as the '{typeof(TResponse).Name}' response via the required '{configureOperation.Parameters[0].Name}' parameter.",
            Tags = [new() { Name = PluralModelName }]
        })
        .Produces<TResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
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
        IEnumerable<TResponse> responses = PaginationFunctions.IsPaginationQueryValidOrNotRequested<TResponse>(paginationQuery) switch
        {
            false => throw new PaginationQueryParametersException(paginationQuery!),
            true => await GetResponsesWithPaginationAsync(httpContext, paginationQuery!).ConfigureAwait(false),
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
        IParameter idParameter = ModelIdService.IdParameter<TModel>(id);
        TModel? model = await QueryBusinessLogic.SelectModelAsync(id, idParameter).ConfigureAwait(false);
        if (model != null)
        {
            TResponse response = QueryMappingService.ModelToResponse(model);
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
        IEnumerable<TResponse> responses = QueryMappingService.ModelsToResponses(models);
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
        IEnumerable<TResponse> responses = QueryMappingService.ModelsToResponses(models);
        PropertyDescriptor orderByProperty = PaginationFunctions.OrderByProperty<TResponse>(paginationQuery);
        int takeQuantity = PaginationFunctions.TakeQuantity(paginationQuery);
        int skipNumber = PaginationFunctions.SkipNumber(paginationQuery);

        if (paginationQuery.OrderByAscending ?? true == true)
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configureOperation"></param>
    /// <returns></returns>
    private OpenApiOperation BuildGetAllOpenApi(OpenApiOperation configureOperation)
    {
        IList<OpenApiParameter> parameters = configureOperation.Parameters;
        StringBuilder openApiStringBuilder = new();
        openApiStringBuilder.Append("Gets a collection of models as '");
        openApiStringBuilder.Append(typeof(TResponse).Name);
        openApiStringBuilder.Append("' responses. Optional pagination is provided via the parameters '");
        openApiStringBuilder.Append(parameters[0].Name);
        openApiStringBuilder.Append("', '");
        openApiStringBuilder.Append(parameters[1].Name);
        openApiStringBuilder.Append("', '");
        openApiStringBuilder.Append(parameters[2].Name);
        openApiStringBuilder.Append("' and '");
        openApiStringBuilder.Append(parameters[3].Name);
        openApiStringBuilder.Append("'.");
        OpenApiOperation openApiOperation = new(configureOperation)
        {
            Summary = openApiStringBuilder.ToString(),
            Tags = [new() { Name = PluralModelName }]
        };
        return openApiOperation;
    }

    #endregion
}