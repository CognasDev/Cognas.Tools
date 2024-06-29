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
    [JsonPropertyName("bpm")]
    public double? Bpm { get; set; }

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