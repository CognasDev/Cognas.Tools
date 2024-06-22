using Cognas.ApiTools.Mapping;

namespace Samples.MusicCollection.Api.Albums;

/// <summary>
/// 
/// </summary>
public sealed class AlbumMappingService : CommandMappingServiceBase<Album, AlbumRequest, AlbumResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="AlbumMappingService"/>
    /// </summary>
    public AlbumMappingService()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public override Album RequestToModel(AlbumRequest request)
    {
        Album model = new()
        {
            AlbumId = request.AlbumId ?? 0,
            ArtistId = request.ArtistId,
            GenreId = request.GenreId,
            LabelId = request.LabelId,
            Name = request.Name,
            ReleaseDate = request.ReleaseDate
        };
        return model;
    }

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