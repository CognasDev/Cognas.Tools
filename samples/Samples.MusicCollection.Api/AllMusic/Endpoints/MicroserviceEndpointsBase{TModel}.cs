using Cognas.ApiTools.Shared.Services;
using Samples.MusicCollection.Api.Albums;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public abstract class MicroserviceEndpointsBase<TModel> where TModel : class
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public virtual int ApiVersion { get; } = 3;

    /// <summary>
    /// 
    /// </summary>
    public string LowerPluralModelName { get; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string Route { get; } = "allmusic";

    /// <summary>
    /// 
    /// </summary>
    public virtual string Tag { get; } = "All Music";

    /// <summary>
    /// 
    /// </summary>
    public string Uri => $"{Route}/{LowerPluralModelName}";

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="MicroserviceEndpointsBase{TModel}"/>
    /// </summary>
    protected MicroserviceEndpointsBase()
    {
        LowerPluralModelName = PluralsService.Instance.PluraliseModelName<TModel>().ToLowerInvariant();
    }

    #endregion
}