using Cognas.ApiTools.Services;

namespace Cognas.ApiTools.Microservices;

/// <summary>
/// 
/// </summary>
public interface ICommandMicroserviceBusinessLogic<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : class
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<LocationResponse<TResponse>> PostAsync(TRequest request);

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