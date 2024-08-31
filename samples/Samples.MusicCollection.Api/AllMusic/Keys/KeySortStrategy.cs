using Samples.MusicCollection.Api.AllMusic.Albums;
using Samples.MusicCollection.Api.AllMusic.Artists;

namespace Samples.MusicCollection.Api.AllMusic.Keys;

/// <summary>
/// 
/// </summary>
public sealed class KeySortStrategy : ISortStrategy
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="KeySortStrategy"/>
    /// </summary>
    public KeySortStrategy()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artists"></param>
    /// <returns></returns>
    public IEnumerable<ArtistAlbumsResponse> SortArtists(IEnumerable<ArtistAlbumsResponse> artists) => artists.OrderBy(artist => artist.Name);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="albums"></param>
    /// <returns></returns>
    public IEnumerable<ArtistAlbumResponse> SortAlbums(IEnumerable<ArtistAlbumResponse> albums) => albums.OrderBy(album => album.Name);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tracks"></param>
    /// <returns></returns>
    public IEnumerable<AlbumTrackResponse> SortTracks(IEnumerable<AlbumTrackResponse> tracks) => tracks.OrderBy(track => track.Key).ThenBy(track => track.TrackNumber);

    #endregion
}