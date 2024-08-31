namespace Samples.MusicCollection.Api.AllMusic;

/// <summary>
/// 
/// </summary>
public interface IAllMusicEndpoints
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    void MapGet(IEndpointRouteBuilder endpointRouteBuilder);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    void MapPostAreMixableTracks(IEndpointRouteBuilder endpointRouteBuilder);

    #endregion
}