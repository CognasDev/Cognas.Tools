using Cognas.ApiTools.SourceGenerators.Attributes;

namespace Samples.MusicCollection.Api.Tracks;

/// <summary>
/// 
/// </summary>
[IncludeInModelIdService]
[CommandScaffold(typeof(TrackRequest), typeof(TrackResponse), 1, false, true)]
[QueryScaffold(typeof(TrackResponse), 1)]
public sealed record Track
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [Id]
    public required int TrackId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required int AlbumId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required int GenreId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? KeyId { get; set; }

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
    /// Default constructor for <see cref="Track"/>
    /// </summary>
    public Track()
    {
    }

    #endregion
}