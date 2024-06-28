using Cognas.ApiTools.Mapping;

namespace Samples.MusicCollection.Api.Tracks;

/// <summary>
/// 
/// </summary>
public sealed class TrackQueryMappingService : QueryMappingServiceBase<Track, TrackResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="TrackQueryMappingService"/>
    /// </summary>
    public TrackQueryMappingService()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public override TrackResponse ModelToResponse(Track model)
    {
        TrackResponse response = new()
        {
            TrackId = model.TrackId,
            AlbumId = model.AlbumId,
            GenreId = model.GenreId,
            KeyId = model.KeyId,
            TrackNumber = model.TrackNumber,
            Name = model.Name,
            Bpm = model.Bpm
        };
        return response;
    }

    #endregion
}