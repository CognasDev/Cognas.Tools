namespace Cognas.ApiTools.Services;

/// <summary>
/// 
/// </summary>
public interface IHttpClientService
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="requestUri"></param>
    /// <returns></returns>
    Task<TItem?> GetAsync<TItem>(string requestUri);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<TItem> GetAsyncEnumerable<TItem>(string requestUri, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    Task<TItem?> PostAsync<TItem>(string requestUri, TItem item);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    Task<TItem?> PutAsync<TItem>(string requestUri, TItem item);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestUri"></param>
    /// <returns></returns>
    Task DeleteAsync<TItem>(string requestUri);

    #endregion
}