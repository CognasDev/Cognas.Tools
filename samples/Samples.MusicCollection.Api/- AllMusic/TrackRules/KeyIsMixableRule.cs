using Samples.MusicCollection.Api.Tracks;

namespace Samples.MusicCollection.Api.AllMusic.TrackRules;

/// <summary>
/// 
/// </summary>
public sealed class KeyIsMixableRule : IMixableTracksRule
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="KeyIsMixableRule"/>
    /// </summary>
    public KeyIsMixableRule()
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
    public bool IsMixable(TrackRequest trackA, TrackRequest trackB)
    {
        if (!trackA.KeyId.HasValue && !trackB.KeyId.HasValue)
        {
            return false;
        }
        int keyDifference = Math.Abs(trackA.KeyId!.Value - trackB.KeyId!.Value);
        bool isMixable = keyDifference < 2 || keyDifference > 22;
        return isMixable;
    }

    #endregion
}