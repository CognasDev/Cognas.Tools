﻿namespace Samples.MusicCollection.Api.AllMusic.Expressions;

/// <summary>
/// 
/// </summary>
public sealed record FlattenedAlbum
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public required int AlbumId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required int ArtistId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? GenreName { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required string LabelName { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required DateTime ReleaseDate { get; init; }

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