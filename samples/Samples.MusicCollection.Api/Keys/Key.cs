using Cognas.ApiTools.SourceGenerators.Attributes;

namespace Samples.MusicCollection.Api.Keys;

/// <summary>
/// 
/// </summary>
[IncludeInModelIdService]
[QueryScaffold(typeof(KeyResponse), 2)]
public sealed record Key
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [Id]
    public required int KeyId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string CamelotCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; set; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="Key"/>
    /// </summary>
    public Key()
    {
    }

    #endregion
}