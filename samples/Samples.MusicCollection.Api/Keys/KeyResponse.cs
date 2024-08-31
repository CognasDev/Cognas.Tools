using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Samples.MusicCollection.Api.Keys;

/// <summary>
/// 
/// </summary>
public sealed record KeyResponse
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("keyId")]
    [Required]
    public required int KeyId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("camelotCode")]
    [Required]
    [StringLength(3)]
    public required string CamelotCode { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("name")]
    [Required]
    [StringLength(250)]
    public required string Name { get; init; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="KeyResponse"/>
    /// </summary>
    public KeyResponse()
    {
    }

    #endregion
}