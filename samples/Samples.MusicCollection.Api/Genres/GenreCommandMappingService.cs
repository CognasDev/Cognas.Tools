using Cognas.ApiTools.Mapping;

namespace Samples.MusicCollection.Api.Genres;

/// <summary>
/// 
/// </summary>
public sealed class GenreCommandMappingService : CommandMappingServiceBase<Genre, GenreRequest, GenreResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="GenreCommandMappingService"/>
    /// </summary>
    public GenreCommandMappingService()
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

    #endregion
}