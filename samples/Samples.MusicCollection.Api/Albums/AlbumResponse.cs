using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Samples.MusicCollection.Api.Albums;

/// <summary>
/// 
/// </summary>
public sealed record AlbumResponse
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("albumId")]
    [Required]
    public required int AlbumId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("artistId")]
    [Required]
    public required int ArtistId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("genreId")]
    [Required]
    public int? GenreId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("labelId")]
    [Required]
    public required int LabelId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("name")]
    [StringLength(250)]
    [Required(AllowEmptyStrings = false)]
    public required string Name { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("releaseDate")]
    [Required]
    public required DateTime ReleaseDate { get; init; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="AlbumResponse"/>
    /// </summary>
    public AlbumResponse()
    {
    }

    #endregion
}