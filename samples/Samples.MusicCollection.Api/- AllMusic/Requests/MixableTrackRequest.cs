using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Samples.MusicCollection.Api.AllMusic.Requests;

/// <summary>
/// 
/// </summary>
public sealed record MixableTrackRequest
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("trackId")]
    [Required]
    public required int TrackId { get; init; }

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
    [JsonPropertyName("bpm")]
    public double? Bpm { get; init; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="MixableTrackRequest"/>
    /// </summary>
    public MixableTrackRequest()
    {
    }

    #endregion
}