using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Samples.MusicCollection.Api.Tracks;

/// <summary>
/// 
/// </summary>
public sealed record TrackResponse
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("trackId")]
    [Required]
    public required int TrackId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("albumId")]
    [Required]
    public required int AlbumId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("genreId")]
    [Required]
    public required int GenreId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("keyId")]
    public int? KeyId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("trackNumber")]
    [Required]
    [Range(1, int.MaxValue)]
    public required int TrackNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("name")]
    [Required]
    [StringLength(250)]
    public required string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("bpm")]
    public double? Bpm { get; set; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="TrackResponse"/>
    /// </summary>
    public TrackResponse()
    {
    }

    #endregion
}