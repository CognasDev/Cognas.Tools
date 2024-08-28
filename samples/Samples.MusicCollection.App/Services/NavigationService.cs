using Samples.MusicCollection.App.Artists;

namespace Samples.MusicCollection.App.Services;

/// <summary>
/// 
/// </summary>
public sealed class NavigationService : INavigationService
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    public void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(ArtistView), typeof(ArtistView));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artist"></param>
    public async Task ToArtistViewAsync(Artist artist)
    {
        Dictionary<string, object> parameters = new(1) { { nameof(Artist), artist } };
        await Shell.Current.GoToAsync(nameof(ArtistView), true, parameters).ConfigureAwait(false);
    }

    #endregion
}