using Cognas.ApiTools.Data;
using Cognas.ApiTools.Shared;

namespace Cognas.ApiTools.SourceGenerators;

/// <summary>
/// Auto generated service used to get and set the property on a model decorated with <see cref="Cognas.ApiTools.SourceGenerators.Attributes.IdAttribute"/>.
/// NB: Should a property be missing the above attribute, source generation will fail and therefore the solution will not build.
/// </summary>
#nullable enable
public sealed partial class ModelIdService : IModelIdService
{{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ModelIdService"/>
    /// </summary>
    public ModelIdService()
    {{
    }}

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public int GetId<TModel>(TModel model) where TModel : class
    {{
        return model switch
        {{
{0}
            _ => throw new NotSupportedException(typeof(TModel).Name)
        }};
    }}

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="model"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public void SetId<TModel>(TModel model, int id) where TModel : class
    {{
        switch (typeof(TModel))
        {{
{1}
            default:
                throw new NotSupportedException(typeof(TModel).Name);
        }};
    }}

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public IParameter IdParameter<TModel>(int id) where TModel : class
    {{
        switch (typeof(TModel))
        {{
{2}
            default:
                throw new NotSupportedException(typeof(TModel).Name);
        }};
    }}

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public string GetModelIdName<TModel>() where TModel : class
    {{
        switch (typeof(TModel))
        {{
{3}
            default:
                throw new NotSupportedException(typeof(TModel).Name);
        }};
    }}

    #endregion
}}