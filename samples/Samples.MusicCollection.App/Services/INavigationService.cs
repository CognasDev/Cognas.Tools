using Samples.MusicCollection.App.Artists;

namespace Samples.MusicCollection.App.Services;

/// <summary>
/// 
/// </summary>
public interface INavigationService
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    void RegisterRoutes();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artist"></param>
    Task ToArtistViewAsync(Artist artist);

    #endregion
}