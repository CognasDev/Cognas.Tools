using Samples.MusicCollection.Api.AllMusic.Strategies;

namespace Samples.MusicCollection.Api.AllMusic.Responses;

/// <summary>
/// 
/// </summary>
public sealed record ArtistAlbumsResponse
{
    #region Field Declarations

    private List<ArtistAlbumResponse>? _albumResponses;

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<ArtistAlbumResponse> Albums => _albumResponses ?? [];

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ArtistAlbumsResponse"/>
    /// </summary>
    public ArtistAlbumsResponse()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="albumResponses"></param>
    /// <param name="sortStrategy"></param>
    public void AddAlbums(IEnumerable<ArtistAlbumResponse> albumResponses, ISortStrategy sortStrategy)
    {
        IEnumerable<ArtistAlbumResponse> sortedAlbums = sortStrategy.SortAlbums(albumResponses);
        _albumResponses = new(sortedAlbums);
    }

    #endregion
}