using Cognas.ApiTools.Pagination;
using Samples.MusicCollection.Api.Albums;

namespace Samples.MusicCollection.Api.AllMusic.BusinessLogic;

/// <summary>
/// 
/// </summary>
public interface IAlbumMicroserviceBusinessLogic
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paginationQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<AlbumResponse> Get(IPaginationQuery paginationQuery, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AlbumResponse?> GetByIdAsync(int id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="album"></param>
    /// <returns></returns>
    Task<AlbumResponse?> PostAsync(AlbumRequest album);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="album"></param>
    /// <returns></returns>
    Task<AlbumResponse?> PutAsync(AlbumRequest album);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="albumId"></param>
    /// <returns></returns>
    Task DeleteAsync(int albumId);

    #endregion
}