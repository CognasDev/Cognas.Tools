using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Cognas.ApiTools.Pagination;

/// <summary>
/// 
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record PaginationQuery : IPaginationQuery
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [FromQuery(Name = "pageSize")]
    [JsonPropertyName("pageSize")]
    public required int? PageSize { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [FromQuery(Name = "pageNumber")]
    [JsonPropertyName("pageNumber")]
    public required int? PageNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [FromQuery(Name = "orderBy")]
    [JsonPropertyName("orderBy")]
    [StringLength(250)]
    public required string? OrderBy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [FromQuery(Name = "orderByAscending")]
    [JsonPropertyName("orderByAscending")]
    public required bool? OrderByAscending { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static PaginationQuery Empty { get;} = new ()
    {
        PageSize = null,
        PageNumber = null,
        OrderBy = null,
        OrderByAscending = null
    };

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="PaginationQuery"/>
    /// </summary>
    public PaginationQuery()
    {
    }

    #endregion
}