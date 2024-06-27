using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Services;
using Microsoft.Extensions.Options;
using System.Text;

namespace Samples.MusicCollection.Api.AllMusic.BusinessLogic;

/// <summary>
/// 
/// </summary>
public abstract class MicroserviceBusinessLogicBase<TRequest, TResponse> : LoggerBusinessLogicBase, IDisposable
    where TRequest : class
    where TResponse : class
{
    #region Field Declarations

    private readonly IHttpClientService _httpClientService;
    private readonly IPaginationFunctions _paginationFunctions;
    private MicroserviceUris _microserviceUris = new();
    private readonly IDisposable? _microserviceUrisMonitorChangedListener;
    private bool _isDisposed;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="MicroserviceBusinessLogicBase{TRequest, TResponse}"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientService"></param>
    /// <param name="microserviceUrisMonitor"></param>
    /// <param name="paginationFunctions"></param>
    public MicroserviceBusinessLogicBase(ILogger logger,
                                         IHttpClientService httpClientService,
                                         IOptionsMonitor<MicroserviceUris> microserviceUrisMonitor,
                                         IPaginationFunctions paginationFunctions) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(httpClientService, nameof(httpClientService));
        ArgumentNullException.ThrowIfNull(microserviceUrisMonitor, nameof(microserviceUrisMonitor));
        ArgumentNullException.ThrowIfNull(paginationFunctions, nameof(paginationFunctions));

        _httpClientService = httpClientService;
        _paginationFunctions = paginationFunctions;

        _microserviceUrisMonitorChangedListener = microserviceUrisMonitor.OnChange(OnMicroserviceUrisChanged);
        _microserviceUris = microserviceUrisMonitor.CurrentValue;
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
    public IAsyncEnumerable<TResponse> Get(IPaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        bool? paginationQueryValidOrDefault = _paginationFunctions.IsPaginationQueryValidOrNotRequested<TRequest>(paginationQuery);
        string requestUri = paginationQueryValidOrDefault == true ? BuildPaginatedQueryString(paginationQuery, MicroserviceUri(_microserviceUris)) : MicroserviceUri(_microserviceUris);
        return _httpClientService.GetAsyncEnumerable<TResponse>(requestUri, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TResponse?> GetByIdAsync(int id)
    {
        string requestUri = $"{MicroserviceUri(_microserviceUris)}/{id}";
        TResponse? album = await _httpClientService.GetAsync<TResponse>(requestUri).ConfigureAwait(false);
        return album;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<TResponse?> PostAsync(TRequest request)
    {
        string requestUri = $"{MicroserviceUri(_microserviceUris)}";
        TResponse? postedResponse = await _httpClientService.PostAsync<TRequest, TResponse>(requestUri, request).ConfigureAwait(false);
        return postedResponse;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<TResponse?> PutAsync(TRequest request)
    {
        string requestUri = $"{MicroserviceUri(_microserviceUris)}";
        TResponse? putResponse = await _httpClientService.PutAsync<TRequest, TResponse>(requestUri, request).ConfigureAwait(false);
        return putResponse;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
    {
        string requestUri = $"{MicroserviceUri(_microserviceUris)}/{id}";
        await _httpClientService.DeleteAsync(requestUri).ConfigureAwait(false);
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
    private void OnMicroserviceUrisChanged(MicroserviceUris microserviceUris) => _microserviceUris = (MicroserviceUris)microserviceUris;

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
            _microserviceUrisMonitorChangedListener?.Dispose();
        }
        _isDisposed = true;
    }

    #endregion
}