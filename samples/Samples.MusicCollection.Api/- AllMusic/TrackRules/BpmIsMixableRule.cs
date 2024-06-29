using Samples.MusicCollection.Api.AllMusic.Requests;

namespace Samples.MusicCollection.Api.AllMusic.TrackRules;

/// <summary>
/// 
/// </summary>
public sealed class BpmIsMixableRule : IMixableTracksRule
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="BpmIsMixableRule"/>
    /// </summary>
    public BpmIsMixableRule()
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
        if (!trackA.Bpm.HasValue && !trackB.Bpm.HasValue)
        {
            return false;
        }
        double mixRangePercentage = 0.08;

        double trackABpm = trackA.Bpm!.Value;
        double trackBBpm = trackB.Bpm!.Value;

        double minimumBpm = trackABpm * (1 - mixRangePercentage);
        double maximumBpm = trackABpm * (1 + mixRangePercentage);

        bool isMixable = trackBBpm >= minimumBpm && trackBBpm <= maximumBpm;
        return isMixable;
    }

    #endregion
}