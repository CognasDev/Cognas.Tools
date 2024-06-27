using Cognas.ApiTools.Pagination;
using Samples.MusicCollection.Api.Artists;

namespace Samples.MusicCollection.Api.AllMusic.BusinessLogic;

/// <summary>
/// 
/// </summary>
public interface IArtistMicroserviceBusinessLogic
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paginationQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ArtistResponse> Get(IPaginationQuery paginationQuery, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ArtistResponse?> GetByIdAsync(int id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artist"></param>
    /// <returns></returns>
    Task<ArtistResponse?> PostAsync(ArtistRequest artist);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artist"></param>
    /// <returns></returns>
    Task<ArtistResponse?> PutAsync(ArtistRequest artist);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artistId"></param>
    /// <returns></returns>
    Task DeleteAsync(int artistId);

    #endregion
}