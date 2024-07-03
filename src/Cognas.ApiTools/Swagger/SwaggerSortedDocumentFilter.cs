using Cognas.Tools.Shared.Extensions;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cognas.ApiTools.Swagger;

/// <summary>
/// 
/// </summary>
public sealed class SwaggerSortedDocumentFilter : IDocumentFilter
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="SwaggerSortedDocumentFilter"/>
    /// </summary>
    public SwaggerSortedDocumentFilter()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="swaggerDoc"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        OpenApiPaths sortedApiPaths = [];
        swaggerDoc.Paths.FastForEach(path =>
        {
            OpenApiPathItem openApiPathItem = new(path.Value);
            openApiPathItem.Operations.Clear();
            path.Value.Operations.OrderBy(operation => operation.Key.GetDisplayName()).FastForEach(operation => openApiPathItem.AddOperation(operation.Key, operation.Value));
            sortedApiPaths.Add(path.Key, openApiPathItem);
        });
        swaggerDoc.Paths = sortedApiPaths;
    }

    #endregion
}