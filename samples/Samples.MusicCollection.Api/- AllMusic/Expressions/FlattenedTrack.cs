namespace Samples.MusicCollection.Api.AllMusic.Expressions;

/// <summary>
/// 
/// </summary>
public sealed record FlattenedTrack
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public required int AlbumId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string GenreName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? KeyName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CamelotCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required int TrackNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double? Bpm { get; set; }

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