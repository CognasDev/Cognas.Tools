using Samples.MusicCollection.Api.AllMusic.Abstractions;

namespace Samples.MusicCollection.Api.AllMusic.Responses;

/// <summary>
/// 
/// </summary>
public sealed record ArtistAlbumResponse
{
    #region Field Declarations

    private List<AlbumTrackResponse>? _trackResponses;

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Genre { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string Label { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required DateTime ReleaseDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<AlbumTrackResponse> Tracks => _trackResponses ?? [];

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ArtistAlbumResponse"/>
    /// </summary>
    public ArtistAlbumResponse()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trackResponses"></param>
    /// <param name="sortStrategy"></param>
    public void AddTracks(IEnumerable<AlbumTrackResponse> trackResponses, ISortStrategy sortStrategy)
    {
        IEnumerable<AlbumTrackResponse> sortedTracks = sortStrategy.SortTracks(trackResponses);
        _trackResponses = new(sortedTracks);
    }

    #endregion
}