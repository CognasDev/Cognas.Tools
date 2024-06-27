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
    IAsyncEnumerable<AlbumResponse> GetAlbums(IPaginationQuery paginationQuery, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="albumId"></param>
    /// <returns></returns>
    Task<AlbumResponse?> GetAlbumByAlbumIdAsync(int albumId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="album"></param>
    /// <returns></returns>
    Task<AlbumResponse?> PostAlbumAsync(AlbumResponse album);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="album"></param>
    /// <returns></returns>
    Task<AlbumResponse?> PutAlbumAsync(AlbumResponse album);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="albumId"></param>
    /// <returns></returns>
    Task DeleteAlbumAsync(int albumId);

    #endregion
}