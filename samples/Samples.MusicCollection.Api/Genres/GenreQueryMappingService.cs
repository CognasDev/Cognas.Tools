using Cognas.ApiTools.Mapping;

namespace Samples.MusicCollection.Api.Genres;

/// <summary>
/// 
/// </summary>
public sealed class GenreQueryMappingService : QueryMappingServiceBase<Genre, GenreResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="GenreQueryMappingService"/>
    /// </summary>
    public GenreQueryMappingService()
    {
    }

    #endregion

    #region Public Method Declarations

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