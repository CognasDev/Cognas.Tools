namespace Samples.MusicCollection.Api.AllMusic.Tracks;

/// <summary>
/// 
/// </summary>
public sealed record FlattenedTrack
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public required int AlbumId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required string GenreName { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? KeyName { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? CamelotCode { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required int TrackNumber { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public double? Bpm { get; init; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="FlattenedTrack"/>
    /// </summary>
    public FlattenedTrack()
    {
    }

    #endregion
}