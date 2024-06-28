namespace Samples.MusicCollection.Api.Config;

/// <summary>
/// 
/// </summary>
public sealed record AllMusicRoutes
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
    /// Default constructor for <see cref="AllMusicRoutes"/>
    /// </summary>
    public AllMusicRoutes()
    {
    }

    #endregion
}