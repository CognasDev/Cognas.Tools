using System.Diagnostics.CodeAnalysis;

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
    public required int? PageSize { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required int? PageNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string? OrderBy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required bool OrderByAscending { get; set; } = true;

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