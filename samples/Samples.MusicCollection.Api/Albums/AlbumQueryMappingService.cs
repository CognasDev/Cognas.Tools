using Cognas.ApiTools.Mapping;

namespace Samples.MusicCollection.Api.Albums;

/// <summary>
/// 
/// </summary>
public sealed class AlbumQueryMappingService : QueryMappingServiceBase<Album, AlbumResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="AlbumQueryMappingService"/>
    /// </summary>
    public AlbumQueryMappingService()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public override AlbumResponse ModelToResponse(Album model)
    {
        AlbumResponse response = new()
        {
            AlbumId = model.AlbumId,
            ArtistId = model.ArtistId,
            GenreId = model.GenreId,
            LabelId = model.LabelId,
            Name = model.Name,
            ReleaseDate = model.ReleaseDate
        };
        return response;
    }

    #endregion
}