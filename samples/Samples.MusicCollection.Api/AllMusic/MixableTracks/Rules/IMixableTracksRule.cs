namespace Samples.MusicCollection.Api.AllMusic.MixableTracks.Rules;

/// <summary>
/// 
/// </summary>
public interface IMixableTracksRule
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trackA"></param>
    /// <param name="trackB"></param>
    bool IsMixable(MixableTrackRequest trackA, MixableTrackRequest trackB);

    #endregion
}