using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Samples.MusicCollection.Api.AllMusic.Albums;

/// <summary>
/// 
/// </summary>
public sealed record AlbumTrackResponse
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("trackNumber")]
    [Required]
    public required int TrackNumber { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("name")]
    [Required(AllowEmptyStrings = false)]
    [StringLength(250)]
    public required string Name { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("genre")]
    [Required(AllowEmptyStrings = false)]
    [StringLength(250)]
    public required string Genre { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("bpm")]
    [Required]
    public required double? Bpm { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("camelotCode")]
    [Required(AllowEmptyStrings = false)]
    [StringLength(3)]
    public string? CamelotCode { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("key")]
    [Required(AllowEmptyStrings = false)]
    [StringLength(250)]
    public string? Key { get; init; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="AlbumTrackResponse"/>
    /// </summary>
    public AlbumTrackResponse()
    {
    }

    #endregion
}