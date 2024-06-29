namespace Samples.MusicCollection.Api.AllMusic.Expressions;

/// <summary>
/// 
/// </summary>
public sealed record FlattenedAlbum
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public required int AlbumId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required int ArtistId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? GenreName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string LabelName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required DateTime ReleaseDate { get; set; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="FlattenedAlbum"/>
    /// </summary>
    public FlattenedAlbum()
    {
    }

    #endregion
}