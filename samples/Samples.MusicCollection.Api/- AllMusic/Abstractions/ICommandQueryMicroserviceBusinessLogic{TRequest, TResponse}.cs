using Cognas.ApiTools.Pagination;

namespace Samples.MusicCollection.Api.AllMusic.Abstractions;

/// <summary>
/// 
/// </summary>
public interface ICommandQueryMicroserviceBusinessLogic<TRequest, TResponse> : IQueryMicroserviceBusinessLogic<TResponse>
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<TResponse?> PostAsync(TRequest request);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<TResponse?> PutAsync(TRequest request);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(int id);

    #endregion
}