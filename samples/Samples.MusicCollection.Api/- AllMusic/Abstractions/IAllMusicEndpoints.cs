using Samples.MusicCollection.Api.AllMusic.Responses;

namespace Samples.MusicCollection.Api.AllMusic.Abstractions;

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

    #endregion
}