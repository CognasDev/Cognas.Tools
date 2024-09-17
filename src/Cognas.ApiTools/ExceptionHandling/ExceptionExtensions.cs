using Cognas.ApiTools.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Cognas.ApiTools.ExceptionHandling;

/// <summary>
/// 
/// </summary>
public static class ExceptionExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddExceptionHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddExceptionHandler<PaginationQueryParametersExceptionHandler>();
        serviceCollection.AddExceptionHandler<OperationCanceledExceptionHandler>();
        serviceCollection.AddExceptionHandler<SqlExceptionHandler>();
        serviceCollection.AddExceptionHandler<GlobalExceptionHandler>();
    }

    #endregion
}