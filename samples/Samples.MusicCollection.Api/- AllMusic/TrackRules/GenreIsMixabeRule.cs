using Samples.MusicCollection.Api.AllMusic.Requests;

namespace Samples.MusicCollection.Api.AllMusic.TrackRules;

/// <summary>
/// 
/// </summary>
public sealed class GenreIsMixabeRule : IMixableTracksRule
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="GenreIsMixabeRule"/>
    /// </summary>
    public GenreIsMixabeRule()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trackA"></param>
    /// <param name="trackB"></param>
    /// <returns></returns>
    public bool IsMixable(MixableTrackRequest trackA, MixableTrackRequest trackB)
    {
        bool isMixable = trackA.GenreId == trackB.GenreId;
        return isMixable;
    }

    #endregion
}