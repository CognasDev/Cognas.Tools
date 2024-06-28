using Cognas.ApiTools.Mapping;

namespace Samples.MusicCollection.Api.Tracks;

/// <summary>
/// 
/// </summary>
public sealed class TrackCommandMappingService : CommandMappingServiceBase<Track, TrackRequest, TrackResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="TrackCommandMappingService"/>
    /// </summary>
    public TrackCommandMappingService()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public override Track RequestToModel(TrackRequest request)
    {
        Track model = new()
        {
            TrackId = request.TrackId ?? 0,
            AlbumId = request.AlbumId,
            GenreId = request.GenreId,
            KeyId = request.KeyId,
            TrackNumber = request.TrackNumber,
            Name = request.Name,
            Bpm = request.Bpm
        };
        return model;
    }

    #endregion
}