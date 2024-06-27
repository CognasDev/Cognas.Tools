using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Services;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Albums;
using System.Text;

namespace Samples.MusicCollection.Api.AllMusic.BusinessLogic;

/// <summary>
/// 
/// </summary>
public sealed class AlbumMicroserviceBusinessLogic : LoggerBusinessLogicBase, IAlbumMicroserviceBusinessLogic, IDisposable
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
    /// Default constructor for <see cref="AlbumMicroserviceBusinessLogic"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientService"></param>
    /// <param name="microserviceUrisMonitor"></param>
    /// <param name="paginationFunctions"></param>
    public AlbumMicroserviceBusinessLogic(ILogger<AlbumMicroserviceBusinessLogic> logger,
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
    /// <param name="paginationQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public IAsyncEnumerable<AlbumResponse> GetAlbums(IPaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        bool? paginationQueryValidOrDefault = _paginationFunctions.IsPaginationQueryValidOrNotRequested<AlbumResponse>(paginationQuery);
        string requestUri = paginationQueryValidOrDefault == true ? BuildPaginatedQueryString(paginationQuery, _microserviceUris.Album) : _microserviceUris.Album;
        return _httpClientService.GetAsyncEnumerable<AlbumResponse>(requestUri, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="albumId"></param>
    /// <returns></returns>
    public async Task<AlbumResponse?> GetAlbumByAlbumIdAsync(int albumId)
    {
        string requestUri = $"{_microserviceUris.Album}/{albumId}";
        AlbumResponse? album = await _httpClientService.GetAsync<AlbumResponse>(requestUri).ConfigureAwait(false);
        return album;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="album"></param>
    /// <returns></returns>
    public async Task<AlbumResponse?> PostAlbumAsync(AlbumResponse album)
    {
        string requestUri = $"{_microserviceUris.Album}";
        AlbumResponse? postedAlbum = await _httpClientService.PostAsync<AlbumResponse>(requestUri, album).ConfigureAwait(false);
        return postedAlbum;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="album"></param>
    /// <returns></returns>
    public async Task<AlbumResponse?> PutAlbumAsync(AlbumResponse album)
    {
        string requestUri = $"{_microserviceUris.Album}";
        AlbumResponse? putAlbum = await _httpClientService.PutAsync<AlbumResponse>(requestUri, album).ConfigureAwait(false);
        return putAlbum;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="albumId"></param>
    /// <returns></returns>
    public async Task DeleteAlbumAsync(int albumId)
    {
        string requestUri = $"{_microserviceUris.Album}/{albumId}";
        await _httpClientService.DeleteAsync<AlbumResponse>(requestUri).ConfigureAwait(false);
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