using Cognas.ApiTools.SourceGenerators.Attributes;

namespace Samples.MusicCollection.Api.Labels;

/// <summary>
/// 
/// </summary>
[IncludeInModelIdService]
[QueryScaffold(typeof(LabelResponse), 1)]
[CommandScaffold(typeof(LabelRequest), typeof(LabelResponse), 1, false)]
public sealed record Label
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [Id]
    public required int LabelId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; set; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="Label"/>
    /// </summary>
    public Label()
    {
    }

    #endregion
}