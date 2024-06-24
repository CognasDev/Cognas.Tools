using Cognas.ApiTools.Extensions;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Cognas.ApiTools.Pagination;

/// <summary>
/// 
/// </summary>
public sealed class PaginationFunctions : IPaginationFunctions
{
    #region Field Declarations

    private readonly ConcurrentDictionary<string, PropertyDescriptor?> _orderByCache = new();

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="PaginationFunctions"/>
    /// </summary>
    public PaginationFunctions()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paginationQuery"></param>
    /// <returns></returns>
    public int TakeQuantity(IPaginationQuery paginationQuery) => paginationQuery.PageSize!.Value;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paginationQuery"></param>
    /// <returns></returns>
    public int SkipNumber(IPaginationQuery paginationQuery) => (paginationQuery.PageNumber!.Value - 1) * TakeQuantity(paginationQuery);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="paginationQuery"></param>
    /// <returns></returns>
    /// <exception cref="PaginationQueryParametersException"></exception>
    public PropertyDescriptor OrderByProperty<TDto>(IPaginationQuery paginationQuery) where TDto : class
    {
        string cacheKey = $"{typeof(TDto).Name}.{paginationQuery.OrderBy}";
        PropertyDescriptor? orderByProperty = _orderByCache.GetOrAdd(cacheKey, key => TypeDescriptor.GetProperties(typeof(TDto)).Find(paginationQuery.OrderBy!, true))
                                              ?? throw new PaginationQueryParametersException(paginationQuery.OrderBy!, typeof(TDto));
        return orderByProperty!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="paginationQuery"></param>
    /// <returns></returns>
    public bool? IsPaginationQueryValidOrNotRequested<TDto>(IPaginationQuery paginationQuery) where TDto : class
    {
        if (!paginationQuery.PageNumber.HasValue && !paginationQuery.PageSize.HasValue && string.IsNullOrWhiteSpace(paginationQuery.OrderBy))
        {
            return null;
        }
        bool isValid = paginationQuery.PageNumber.HasValue &&
                       paginationQuery.PageSize.HasValue &&
                       !string.IsNullOrWhiteSpace(paginationQuery.OrderBy) &&
                       OrderByProperty<TDto>(paginationQuery) != null;
        return isValid;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="paginationQuery"></param>
    /// <param name="models"></param>
    /// <param name="httpContext"></param>
    public void BuildPaginationResponseHeader<TModel>(IPaginationQuery paginationQuery, IEnumerable<TModel> models, HttpContext httpContext) where TModel : class
    {
        bool successfulCount = models.TryGetNonEnumeratedCount(out int modelCount);

        if (!successfulCount)
        {
            modelCount = models.Count();
        }

        int pageCount = ((modelCount - 1) / paginationQuery.PageSize!.Value) + 1;
        IHeaderDictionary headers = httpContext.Response.Headers;

        headers.AppendSanitised("x-total", modelCount);
        headers.AppendSanitised("x-page-count", pageCount);
        headers.AppendSanitised("x-page-size", paginationQuery.PageSize!.Value);
        headers.AppendSanitised("x-page-number", paginationQuery.PageNumber!.Value);
        headers.AppendSanitised($"x-page-{nameof(paginationQuery.OrderBy).ToLower()}", paginationQuery.OrderBy!);
        headers.AppendSanitised($"x-page-{nameof(paginationQuery.OrderByAscending).ToLower()}", paginationQuery.OrderByAscending ?? true);
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="headerValue"></param>
    /// <returns></returns>
    private static string SanitizeHeaderValue(string headerValue)
    {
        string sanitizeHeaderValue = headerValue
                                     .Replace("\r", string.Empty)
                                     .Replace("%0d", string.Empty)
                                     .Replace("%0D", string.Empty)
                                     .Replace("\n", string.Empty)
                                     .Replace("%0a", string.Empty)
                                     .Replace("%0A", string.Empty);
        return sanitizeHeaderValue;
    }

    #endregion
}