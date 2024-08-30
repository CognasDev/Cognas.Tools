using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Samples.MusicCollection.Api.Genres;

/// <summary>
/// 
/// </summary>
public sealed record GenreRequest
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("genreId")]
    public int? GenreId { get; init; }

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
    /// Default constructor for <see cref="GenreRequest"/>
    /// </summary>
    public GenreRequest()
    {
    }

    #endregion
}