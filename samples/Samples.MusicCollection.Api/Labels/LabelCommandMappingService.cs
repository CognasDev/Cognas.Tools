using Cognas.ApiTools.Mapping;

namespace Samples.MusicCollection.Api.Labels;

/// <summary>
/// 
/// </summary>
public sealed class LabelCommandMappingService : CommandMappingServiceBase<Label, LabelRequest>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="LabelCommandMappingService"/>
    /// </summary>
    public LabelCommandMappingService()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public override Label RequestToModel(LabelRequest request)
    {
        Label model = new()
        {
            LabelId = request.LabelId ?? NotInsertedId,
            Name = request.Name
        };
        return model;
    }

    #endregion
}