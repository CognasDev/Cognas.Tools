using Samples.MusicCollection.Api.AllMusic.Abstractions;

namespace Samples.MusicCollection.Api.AllMusic.Responses;

/// <summary>
/// 
/// </summary>
public sealed record AllMusicResponse
{
    #region Field Declarations

    private List<ArtistAlbumsResponse>? _artistResponses;

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<ArtistAlbumsResponse> Artists => _artistResponses ?? [];

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="AllMusicResponse"/>
    /// </summary>
    public AllMusicResponse()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artistResponses"></param>
    /// <param name="sortStrategy"></param>
    public void AddArtists(IEnumerable<ArtistAlbumsResponse> artistResponses, ISortStrategy sortStrategy)
    {
        IEnumerable<ArtistAlbumsResponse> sortedTracks = sortStrategy.SortArtists(artistResponses);
        _artistResponses = new(sortedTracks);
    }

    #endregion
}