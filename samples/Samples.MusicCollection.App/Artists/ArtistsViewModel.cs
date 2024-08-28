using Cognas.MauiTools.Shared.Services;
using Cognas.MaulTools.Shared.Mvvm;
using System.Windows.Input;
using Cognas.MaulTools.Shared.Commands;
using Samples.MusicCollection.App.Services;

namespace Samples.MusicCollection.App.Artists;

/// <summary>
/// 
/// </summary>
public sealed class ArtistsViewModel : ViewModelBase
{
    #region Field Declarations

    private readonly INavigationService _navigationService;

    private ICommand? _getArtistsCommand;
    private ICommand? _ViewArtistCommand;

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public IArtistsRepository ArtistsRepository { get; }

    #endregion

    #region Command Declarations

    /// <summary>
    /// 
    /// </summary>
    public ICommand GetArtistsCommand =>
        _getArtistsCommand ??= new DelegateCommand(async () => await GetArtistsExecuteAsync().ConfigureAwait(false));

    /// <summary>
    /// 
    /// </summary>
    public ICommand ViewArtistCommand =>
        _ViewArtistCommand ??= new DelegateCommand<Artist>(async artist => await ViewArtistExecuteAsync(artist).ConfigureAwait(false));

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ArtistsViewModel"/>
    /// </summary>
    /// <param name="artistsRepository"></param>
    /// <param name="navigationService"></param>
    public ArtistsViewModel(IArtistsRepository artistsRepository, INavigationService navigationService)
    {
        ArgumentNullException.ThrowIfNull(artistsRepository, nameof(artistsRepository));
        ArgumentNullException.ThrowIfNull(navigationService, nameof(navigationService));
        ArtistsRepository = artistsRepository;
        _navigationService = navigationService;
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    private async Task GetArtistsExecuteAsync()
    {
        await ArtistsRepository.InitiateAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artist"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    private async Task ViewArtistExecuteAsync(Artist? artist)
    {
        await _navigationService.ToArtistViewAsync(artist ?? throw new NullReferenceException(nameof(Artist))).ConfigureAwait(false);
    }

    #endregion
}