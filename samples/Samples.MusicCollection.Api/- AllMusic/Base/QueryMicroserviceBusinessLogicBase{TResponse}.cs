using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Microservices;
using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Services;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Config;
using System.Text;

namespace Samples.MusicCollection.Api.AllMusic.Base;

/// <summary>
/// 
/// </summary>
public abstract class QueryMicroserviceBusinessLogicBase<TResponse> :
    LoggerBusinessLogicBase, IDisposable, IQueryMicroserviceBusinessLogic<TResponse>
    where TResponse : class
{
    #region Field Declarations

    private readonly IPaginationFunctions _paginationFunctions;
    private readonly IDisposable? _microserviceUrisChangedListener;
    private bool _isDisposed;

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    protected IHttpClientService HttpClientService { get; }

    /// <summary>
    /// 
    /// </summary>
    protected MicroserviceUris MicroserviceUris { get; private set; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="QueryMicroserviceBusinessLogicBase{TResponse}"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientService"></param>
    /// <param name="microserviceUrisMonitor"></param>
    /// <param name="paginationFunctions"></param>
    protected QueryMicroserviceBusinessLogicBase(ILogger logger,
                                                 IHttpClientService httpClientService,
                                                 IOptionsMonitor<MicroserviceUris> microserviceUrisMonitor,
                                                 IPaginationFunctions paginationFunctions) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(httpClientService, nameof(httpClientService));
        ArgumentNullException.ThrowIfNull(microserviceUrisMonitor, nameof(microserviceUrisMonitor));
        ArgumentNullException.ThrowIfNull(paginationFunctions, nameof(paginationFunctions));

        HttpClientService = httpClientService;
        _paginationFunctions = paginationFunctions;

        _microserviceUrisChangedListener = microserviceUrisMonitor.OnChange(OnMicroserviceUrisChanged);
        MicroserviceUris = microserviceUrisMonitor.CurrentValue;
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="microserviceUris"></param>
    /// <returns></returns>
    public abstract string MicroserviceUri(MicroserviceUris microserviceUris);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paginationQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public IAsyncEnumerable<TResponse> Get(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        bool? paginationQueryValidOrDefault = _paginationFunctions.IsPaginationQueryValidOrNotRequested<TResponse>(paginationQuery);
        string microserviceUri = MicroserviceUri(MicroserviceUris);
        string requestUri = paginationQueryValidOrDefault == true ? BuildPaginatedQueryString(paginationQuery, microserviceUri) : microserviceUri;
        return HttpClientService.GetAsyncEnumerable<TResponse>(requestUri, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TResponse?> GetByIdAsync(int id)
    {
        string requestUri = $"{MicroserviceUri(MicroserviceUris)}/{id}";
        TResponse? response = await HttpClientService.GetAsync<TResponse>(requestUri).ConfigureAwait(false);
        return response;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="microserviceUris"></param>
    private void OnMicroserviceUrisChanged(MicroserviceUris microserviceUris) => MicroserviceUris = microserviceUris;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paginationQuery"></param>
    /// <param name="microserviceUri"></param>
    /// <returns></returns>
    private static string BuildPaginatedQueryString(IPaginationQuery paginationQuery, string microserviceUri)
    {
        StringBuilder paginationQueryStringBuilder = new();
        paginationQueryStringBuilder.Append(microserviceUri);
        paginationQueryStringBuilder.Append('?');
        paginationQueryStringBuilder.Append(nameof(IPaginationQuery.PageSize));
        paginationQueryStringBuilder.Append('=');
        paginationQueryStringBuilder.Append(paginationQuery.PageSize);
        paginationQueryStringBuilder.Append('&');
        paginationQueryStringBuilder.Append(nameof(IPaginationQuery.PageNumber));
        paginationQueryStringBuilder.Append('=');
        paginationQueryStringBuilder.Append(paginationQuery.PageNumber);
        paginationQueryStringBuilder.Append('&');
        paginationQueryStringBuilder.Append(nameof(IPaginationQuery.OrderBy));
        paginationQueryStringBuilder.Append('=');
        paginationQueryStringBuilder.Append(paginationQuery.OrderBy);
        paginationQueryStringBuilder.Append('&');
        paginationQueryStringBuilder.Append(nameof(IPaginationQuery.OrderByAscending));
        paginationQueryStringBuilder.Append('=');
        paginationQueryStringBuilder.Append(paginationQuery.OrderByAscending);
        string paginationQueryString = paginationQueryStringBuilder.ToString();
        return paginationQueryString;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    private void Dispose(bool disposing)
    {
        if (!_isDisposed && disposing)
        {
            _microserviceUrisChangedListener?.Dispose();
        }
        _isDisposed = true;
    }

    #endregion
}