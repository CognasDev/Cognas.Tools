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
    public string Albums { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Artists { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Genres { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Labels { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Keys { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Tracks { get; set; } = null!;

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