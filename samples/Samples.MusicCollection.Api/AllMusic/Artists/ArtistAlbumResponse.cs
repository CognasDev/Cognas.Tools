using Samples.MusicCollection.Api.AllMusic.Albums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Samples.MusicCollection.Api.AllMusic.Artists;

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
    [JsonPropertyName("name")]
    [Required]
    [StringLength(250)]
    public required string Name { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("genre")]
    [StringLength(250)]
    public string? Genre { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("label")]
    [Required]
    [StringLength(250)]
    public required string Label { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("releaseDate")]
    [Required]
    [StringLength(250)]
    public required DateTime ReleaseDate { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("tracks")]
    [Required]
    [StringLength(250)]
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