using Cognas.MaulTools.Shared.Mvvm;

namespace Samples.MusicCollection.App.Artists;

/// <summary>
/// 
/// </summary>
public sealed class ArtistViewModel : ViewModelBase, IQueryAttributable
{
    #region Field Declarations

    private Artist _artist = null!;

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public Artist Artist
    {
        get => _artist;
        set => SetProperty(ref _artist, value);
    }

    #endregion

    #region Command Declarations

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ArtistViewModel"/>
    /// </summary>
    public ArtistViewModel()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="query"></param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Artist = (Artist)query[nameof(Artist)];
    }

    #endregion
}