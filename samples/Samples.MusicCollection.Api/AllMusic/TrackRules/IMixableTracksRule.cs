using Samples.MusicCollection.Api.Tracks;

namespace Samples.MusicCollection.Api.AllMusic.TrackRules;

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
    bool IsMixable(TrackRequest trackA, TrackRequest trackB);

    #endregion
}