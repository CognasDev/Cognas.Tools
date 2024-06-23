using Cognas.ApiTools.Mapping;

namespace Samples.MusicCollection.Api.Labels;

/// <summary>
/// 
/// </summary>
public sealed class LabelQueryMappingService : QueryMappingServiceBase<Label, LabelResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="LabelQueryMappingService"/>
    /// </summary>
    public LabelQueryMappingService()
    {
    }

    #endregion

    #region Public Method Declarations

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