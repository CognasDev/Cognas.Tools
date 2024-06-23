using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Cognas.ApiTools.Services;

/// <summary>
/// 
/// </summary>
public sealed class HttpClientService : IHttpClientService
{
    #region Field Declarations

    private static readonly JsonSerializerOptions _caseInsensitiveSerializer = new() { PropertyNameCaseInsensitive = true };
    private readonly IHttpClientFactory _httpClientFactory;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="HttpClientService"/>
    /// </summary>
    /// <param name="httpClientFactory"></param>
    public HttpClientService(IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));
        _httpClientFactory = httpClientFactory;
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="requestUri"></param>
    /// <returns></returns>
    public async Task<TItem?> GetAsync<TItem>(string requestUri)
    {
        HttpClient httpClient = CreateHttpClient();
        using HttpResponseMessage response = await httpClient.GetAsync(requestUri).ConfigureAwait(false);
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return default;
        }
        response.EnsureSuccessStatusCode();
        using Stream responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        return await JsonSerializer.DeserializeAsync<TItem>(responseStream, _caseInsensitiveSerializer).ConfigureAwait(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public async IAsyncEnumerable<TItem> GetAsyncEnumerable<TItem>(string requestUri, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        HttpClient httpClient = CreateHttpClient();
        using HttpResponseMessage response = await httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
        using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        await foreach (TItem? item in JsonSerializer.DeserializeAsyncEnumerable<TItem>(responseStream, _caseInsensitiveSerializer, cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return item ?? throw new NullReferenceException(nameof(item));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<TItem?> PostAsync<TItem>(string requestUri, TItem item)
    {
        HttpClient httpClient = CreateHttpClient();
        using HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, item).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        using Stream responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        return await JsonSerializer.DeserializeAsync<TItem>(responseStream, _caseInsensitiveSerializer).ConfigureAwait(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<TItem?> PutAsync<TItem>(string requestUri, TItem item)
    {
        HttpClient httpClient = CreateHttpClient();
        using HttpResponseMessage response = await httpClient.PutAsJsonAsync(requestUri, item).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        using Stream responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        return await JsonSerializer.DeserializeAsync<TItem>(responseStream, _caseInsensitiveSerializer).ConfigureAwait(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestUri"></param>
    /// <returns></returns>
    public async Task DeleteAsync<TItem>(string requestUri)
    {
        HttpClient httpClient = CreateHttpClient();
        using HttpResponseMessage response = await httpClient.DeleteAsync(requestUri).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private HttpClient CreateHttpClient()
    {
        HttpClient httpClient = _httpClientFactory.CreateClient();
        return httpClient;
    }

    #endregion
}