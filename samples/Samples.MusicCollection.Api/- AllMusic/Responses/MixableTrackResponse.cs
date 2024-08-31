using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Samples.MusicCollection.Api.AllMusic.Responses;

/// <summary>
/// 
/// </summary>
public sealed record MixableTrackResponse
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("trackAId")]
    [Required]
    public required int TrackAId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("trackBId")]
    [Required]
    public required int TrackBId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("isMixable")]
    [Required]
    public required bool IsMixable { get; init; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="MixableTrackResponse"/>
    /// </summary>
    public MixableTrackResponse()
    {
    }

    #endregion
}