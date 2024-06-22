﻿using Microsoft.AspNetCore.Http;
using System.Text;

namespace Cognas.ApiTools.Pagination;

/// <summary>
/// 
/// </summary>
public sealed class PaginationQueryParametersException : BadHttpRequestException
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Query constructor for <see cref="PaginationQueryParametersException"/>
    /// </summary>
    /// <param name="paginationQuery"></param>
    public PaginationQueryParametersException(IPaginationQuery paginationQuery) : base(BuildExceptionMessage(paginationQuery))
    {
    }

    /// <summary>
    /// OrderBy constructor for <see cref="PaginationQueryParametersException"/>
    /// </summary>
    /// <param name="orderBy"></param>
    /// <param name="dtoType"></param>
    public PaginationQueryParametersException(string orderBy, Type dtoType) :
        base($"{nameof(IPaginationQuery.OrderBy)} parameter value '{orderBy}' is not a property of {dtoType.Name}.")
    {
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paginationQuery"></param>
    /// <returns></returns>
    private static string BuildExceptionMessage(IPaginationQuery paginationQuery)
    {
        StringBuilder messageStringBuilder = new();
        messageStringBuilder.Append(nameof(PaginationQuery));
        messageStringBuilder.Append(" malformed: ");
        if (!paginationQuery.PageNumber.HasValue)
        {
            messageStringBuilder.Append(nameof(IPaginationQuery.PageNumber));
            messageStringBuilder.Append(" parameter not provided. ");
        }
        if (!paginationQuery.PageSize.HasValue)
        {
            messageStringBuilder.Append(nameof(IPaginationQuery.PageSize));
            messageStringBuilder.Append(" parameter not provided. ");
        }
        if (string.IsNullOrWhiteSpace(paginationQuery.OrderBy))
        {
            messageStringBuilder.Append(nameof(IPaginationQuery.OrderBy));
            messageStringBuilder.Append(" parameter not provided.");
        }
        string message = messageStringBuilder.ToString().TrimEnd();
        return message;
    }

    #endregion
}