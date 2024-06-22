namespace Cognas.ApiTools.Mapping;

/// <summary>
/// 
/// </summary>
public sealed class MapRequestToModelNotSupportedException : NotSupportedException
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public string DtoName { get; }

    /// <summary>
    /// 
    /// </summary>
    public string ModelName { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="MapRequestToModelNotSupportedException"/>
    /// </summary>
    /// <param name="dtoType"></param>
    /// <param name="modelType"></param>
    public MapRequestToModelNotSupportedException(Type dtoType, Type modelType) : base($"Mapping from {dtoType.Name} to {modelType.Name} not supported.")
    {
        DtoName = dtoType.Name;
        ModelName = modelType.Name;
    }

    #endregion
}