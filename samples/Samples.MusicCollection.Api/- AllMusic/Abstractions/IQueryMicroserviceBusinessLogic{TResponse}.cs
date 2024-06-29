using Cognas.ApiTools.Pagination;

namespace Samples.MusicCollection.Api.AllMusic.Abstractions;

/// <summary>
/// 
/// </summary>
public interface IQueryMicroserviceBusinessLogic<TResponse> where TResponse : class
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paginationQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<TResponse> Get(PaginationQuery paginationQuery, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TResponse?> GetByIdAsync(int id);

    #endregion
}