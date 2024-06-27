using Cognas.ApiTools.Mapping;

namespace Samples.MusicCollection.Api.Artists;

/// <summary>
/// 
/// </summary>
public sealed class ArtistQueryMappingService : QueryMappingServiceBase<Artist, ArtistResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ArtistQueryMappingService"/>
    /// </summary>
    public ArtistQueryMappingService()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public override ArtistResponse ModelToResponse(Artist model)
    {
        ArtistResponse response = new()
        {
            ArtistId = model.ArtistId,
            Name = model.Name
        };
        return response;
    }

    #endregion
}