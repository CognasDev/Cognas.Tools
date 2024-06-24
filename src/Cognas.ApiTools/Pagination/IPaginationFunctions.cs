using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace Cognas.ApiTools.Pagination;

/// <summary>
/// 
/// </summary>
public interface IPaginationFunctions
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paginationQuery"></param>
    /// <returns></returns>
    int TakeQuantity(IPaginationQuery paginationQuery);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paginationQuery"></param>
    /// <returns></returns>
    int SkipNumber(IPaginationQuery paginationQuery);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="paginationQuery"></param>
    /// <returns></returns>
    /// <exception cref="PaginationQueryParametersException"></exception>
    PropertyDescriptor OrderByProperty<TDto>(IPaginationQuery paginationQuery) where TDto : class;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="paginationQuery"></param>
    /// <returns></returns>
    bool? IsPaginationQueryValidOrNotRequested<TDto>(IPaginationQuery paginationQuery) where TDto : class;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="paginationQuery"></param>
    /// <param name="responses"></param>
    /// <param name="httpContext"></param>
    void BuildPaginationResponseHeader<TDto>(IPaginationQuery paginationQuery, IEnumerable<TDto> responses, HttpContext httpContext) where TDto : class;

    #endregion
}