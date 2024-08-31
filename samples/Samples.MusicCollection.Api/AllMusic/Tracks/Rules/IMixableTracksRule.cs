using Samples.MusicCollection.Api.AllMusic.MixableTracks;
using Samples.MusicCollection.Api.Tracks;

namespace Samples.MusicCollection.Api.AllMusic.Tracks.Rules;

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