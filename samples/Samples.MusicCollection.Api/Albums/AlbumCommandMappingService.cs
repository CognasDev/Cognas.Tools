using Cognas.ApiTools.Mapping;

namespace Samples.MusicCollection.Api.Albums;

/// <summary>
/// 
/// </summary>
public sealed class AlbumCommandMappingService : CommandMappingServiceBase<Album, AlbumRequest, AlbumResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="AlbumCommandMappingService"/>
    /// </summary>
    public AlbumCommandMappingService()
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
            AlbumId = request.AlbumId ?? NotInsertedId,
            ArtistId = request.ArtistId,
            GenreId = request.GenreId,
            LabelId = request.LabelId,
            Name = request.Name,
            ReleaseDate = request.ReleaseDate
        };
        return model;
    }

    #endregion
}