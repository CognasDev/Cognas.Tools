using Samples.MusicCollection.Api.AllMusic.Requests;
using Samples.MusicCollection.Api.AllMusic.Responses;

namespace Samples.MusicCollection.Api.AllMusic.Abstractions;

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