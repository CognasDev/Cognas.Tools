using Cognas.MauiTools.Shared.Services;
using Cognas.MaulTools.Shared.Mvvm;
using System.Windows.Input;
using Cognas.MaulTools.Shared.Commands;

namespace Samples.MusicCollection.App.Artists;

/// <summary>
/// 
/// </summary>
public sealed class ArtistsViewModel : ViewModelBase
{
    #region Field Declarations

    private readonly IArtistsRepository _artistsRepository;
    private ICommand? _getArtistsCommand;

    #endregion

    #region Property Declarations
    #endregion

    #region Command Declarations

    /// <summary>
    /// 
    /// </summary>
    public ICommand GetArtistsCommand =>
        _getArtistsCommand ??= new DelegateCommand(async () => await GetArtistsExecuteAsync().ConfigureAwait(false));

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ArtistsView"/>
    /// </summary>
    /// <param name="artistsRepository"></param>
    public ArtistsViewModel(IArtistsRepository artistsRepository)
    {
        ArgumentNullException.ThrowIfNull(artistsRepository, nameof(artistsRepository));
        _artistsRepository = artistsRepository;
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    private async Task GetArtistsExecuteAsync()
    {
        await _artistsRepository.InitiateAsync().ConfigureAwait(false);
    }

    #endregion
}