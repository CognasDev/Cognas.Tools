using Cognas.ApiTools.SourceGenerators.Attributes;

namespace Samples.MusicCollection.Api.Genres;

/// <summary>
/// 
/// </summary>
[IncludeInModelIdService]
[CommandScaffold(typeof(GenreRequest), typeof(GenreResponse), 1, false)]
[QueryScaffold(typeof(GenreResponse), 1)]
public sealed record Genre
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    [Id]
    public required int GenreId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; set; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="Genre"/>
    /// </summary>
    public Genre()
    {
    }

    #endregion
}