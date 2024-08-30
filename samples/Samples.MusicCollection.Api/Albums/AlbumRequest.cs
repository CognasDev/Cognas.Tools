using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Samples.MusicCollection.Api.Albums;

/// <summary>
/// 
/// </summary>
public sealed record AlbumRequest
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("albumId")]
    public int? AlbumId { get; init; }

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
    [Required]
    [StringLength(250)]
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
    /// Default constructor for <see cref="AlbumRequest"/>
    /// </summary>
    public AlbumRequest()
    {
    }

    #endregion
}