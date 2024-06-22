namespace Samples.MusicCollection.Api;

/// <summary>
/// 
/// </summary>
public sealed record MicroserviceUris
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public string Album { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Artist { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Genre { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Label { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Track { get; set; } = null!;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="MicroserviceUris"/>
    /// </summary>
    public MicroserviceUris()
    {
    }

    #endregion
}