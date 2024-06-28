using Cognas.ApiTools.SourceGenerators.Attributes;

namespace Samples.MusicCollection.Api.Albums;

/// <summary>
/// 
/// </summary>
[IncludeInModelIdService]
[CommandScaffold(typeof(AlbumRequest), typeof(AlbumResponse), 1, false)]
[QueryScaffold(typeof(AlbumResponse), 1)]
public sealed record Album
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [Id]
    public required int AlbumId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required int ArtistId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? GenreId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required int LabelId { get; set; }

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
    /// Default constructor for <see cref="Album"/>
    /// </summary>
    public Album()
    {
    }

    #endregion
}