using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Samples.MusicCollection.Api.Tracks;

/// <summary>
/// 
/// </summary>
public sealed record TrackRequest
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("trackId")]
    public required int? TrackId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("albumId")]
    [Required]
    public required int AlbumId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("genreId")]
    [Required]
    public required int GenreId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("keyId")]
    public int? KeyId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("trackNumber")]
    [Required]
    [Range(1, int.MaxValue)]
    public required int TrackNumber { get; init; }

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
    [JsonPropertyName("bpm")]
    public double? Bpm { get; init; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="TrackRequest"/>
    /// </summary>
    public TrackRequest()
    {
    }

    #endregion
}