using Cognas.ApiTools.Mapping;

namespace Samples.MusicCollection.Api.Labels;

/// <summary>
/// 
/// </summary>
public sealed class LabelMappingService : MappingServiceBase<Label, LabelRequest, LabelResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="LabelMappingService"/>
    /// </summary>
    public LabelMappingService()
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
            LabelId = request.LabelId ?? 0,
            Name = request.Name
        };
        return model;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public override LabelResponse ModelToResponse(Label model)
    {
        LabelResponse response = new()
        {
            LabelId = model.LabelId,
            Name = model.Name
        };
        return response;
    }

    #endregion
}