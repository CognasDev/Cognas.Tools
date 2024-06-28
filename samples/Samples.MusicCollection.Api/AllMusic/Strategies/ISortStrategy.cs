using Samples.MusicCollection.Api.AllMusic.Responses;

namespace Samples.MusicCollection.Api.AllMusic.Strategies;

/// <summary>
/// 
/// </summary>
public interface ISortStrategy
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artists"></param>
    /// <returns></returns>
    IEnumerable<ArtistAlbumsResponse> SortArtists(IEnumerable<ArtistAlbumsResponse> artists);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="albums"></param>
    /// <returns></returns>
    IEnumerable<ArtistAlbumResponse> SortAlbums(IEnumerable<ArtistAlbumResponse> albums);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tracks"></param>
    /// <returns></returns>
    IEnumerable<AlbumTrackResponse> SortTracks(IEnumerable<AlbumTrackResponse> tracks);

    #endregion
}