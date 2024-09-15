using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Samples.MusicCollection.Api.Genres;

/// <summary>
/// 
/// </summary>
public sealed record GenreResponse
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("genreId")]
    [Required]
    public required int GenreId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("name")]
    [Required(AllowEmptyStrings = false)]
    [StringLength(250)]
    public required string Name { get; init; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="GenreResponse"/>
    /// </summary>
    public GenreResponse()
    {
    }

    #endregion
}