using Samples.MusicCollection.Api.AllMusic.MixableTracks;

namespace Samples.MusicCollection.Api.AllMusic;

/// <summary>
/// 
/// </summary>
public interface IAllMusicBusinessLogic
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AllMusicResponse> GetAllMusicAsync(CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mixableTrackRequests"></param>
    /// <returns></returns>
    IEnumerable<MixableTrackResponse> AreMixableTracks(IEnumerable<MixableTrackRequest> mixableTrackRequests);

    #endregion
}