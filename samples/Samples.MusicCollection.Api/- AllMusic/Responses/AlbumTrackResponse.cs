namespace Samples.MusicCollection.Api.AllMusic.Responses;

/// <summary>
/// 
/// </summary>
public sealed record AlbumTrackResponse
{
    #region Property Declarations

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
    public required string Genre { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required double? Bpm { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CamelotCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Key { get; set; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="AlbumTrackResponse"/>
    /// </summary>
    public AlbumTrackResponse()
    {
    }

    #endregion
}