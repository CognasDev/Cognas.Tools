using Cognas.ApiTools.Mapping;

namespace Samples.MusicCollection.Api.Genres;

/// <summary>
/// 
/// </summary>
public sealed class GenreMappingService : CommandMappingServiceBase<Genre, GenreRequest, GenreResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="GenreMappingService"/>
    /// </summary>
    public GenreMappingService()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public override Genre RequestToModel(GenreRequest request)
    {
        Genre model = new()
        {
            GenreId = request.GenreId ?? 0,
            Name = request.Name
        };
        return model;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public override GenreResponse ModelToResponse(Genre model)
    {
        GenreResponse response = new()
        {
            GenreId = model.GenreId,
            Name = model.Name
        };
        return response;
    }

    #endregion
}