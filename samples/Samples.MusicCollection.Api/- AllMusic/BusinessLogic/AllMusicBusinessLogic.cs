using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Shared.Extensions;
using Samples.MusicCollection.Api.Albums;
using Samples.MusicCollection.Api.AllMusic.Abstractions;
using Samples.MusicCollection.Api.AllMusic.Expressions;
using Samples.MusicCollection.Api.AllMusic.Responses;
using Samples.MusicCollection.Api.AllMusic.Strategies;
using Samples.MusicCollection.Api.Artists;
using Samples.MusicCollection.Api.Genres;
using Samples.MusicCollection.Api.Keys;
using Samples.MusicCollection.Api.Labels;
using Samples.MusicCollection.Api.Tracks;
using System.Collections.Concurrent;

namespace Samples.MusicCollection.Api.AllMusic.BusinessLogic;

/// <summary>
/// 
/// </summary>
public sealed class AllMusicBusinessLogic : IAllMusicBusinessLogic
{
    #region Field Declarations

    private readonly IQueryMicroserviceBusinessLogic<AlbumResponse> _albumsQueryBusinessLogic;
    private readonly IQueryMicroserviceBusinessLogic<ArtistResponse> _artistsQueryBusinessLogic;
    private readonly IQueryMicroserviceBusinessLogic<GenreResponse> _genresQueryBusinessLogic;
    private readonly IQueryMicroserviceBusinessLogic<KeyResponse> _keysQueryBusinessLogic;
    private readonly IQueryMicroserviceBusinessLogic<LabelResponse> _labelsQueryBusinessLogic;
    private readonly IQueryMicroserviceBusinessLogic<TrackResponse> _tracksQueryBusinessLogic;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="AllMusicBusinessLogic"/>
    /// </summary>
    /// <param name="albumsQueryBusinessLogic"></param>
    /// <param name="artistsQueryBusinessLogic"></param>
    /// <param name="genresQueryBusinessLogic"></param>
    /// <param name="keysQueryBusinessLogic"></param>
    /// <param name="labelsQueryBusinessLogic"></param>
    /// <param name="tracksQueryBusinessLogic"></param>
    public AllMusicBusinessLogic(IQueryMicroserviceBusinessLogic<AlbumResponse> albumsQueryBusinessLogic,
                                 IQueryMicroserviceBusinessLogic<ArtistResponse> artistsQueryBusinessLogic,
                                 IQueryMicroserviceBusinessLogic<GenreResponse> genresQueryBusinessLogic,
                                 IQueryMicroserviceBusinessLogic<KeyResponse> keysQueryBusinessLogic,
                                 IQueryMicroserviceBusinessLogic<LabelResponse> labelsQueryBusinessLogic,
                                 IQueryMicroserviceBusinessLogic<TrackResponse> tracksQueryBusinessLogic)
    {
        ArgumentNullException.ThrowIfNull(albumsQueryBusinessLogic, nameof(albumsQueryBusinessLogic));
        ArgumentNullException.ThrowIfNull(artistsQueryBusinessLogic, nameof(artistsQueryBusinessLogic));
        ArgumentNullException.ThrowIfNull(genresQueryBusinessLogic, nameof(genresQueryBusinessLogic));
        ArgumentNullException.ThrowIfNull(keysQueryBusinessLogic, nameof(keysQueryBusinessLogic));
        ArgumentNullException.ThrowIfNull(labelsQueryBusinessLogic, nameof(labelsQueryBusinessLogic));
        ArgumentNullException.ThrowIfNull(tracksQueryBusinessLogic, nameof(tracksQueryBusinessLogic));

        _albumsQueryBusinessLogic = albumsQueryBusinessLogic;
        _artistsQueryBusinessLogic = artistsQueryBusinessLogic;
        _genresQueryBusinessLogic = genresQueryBusinessLogic;
        _keysQueryBusinessLogic = keysQueryBusinessLogic;
        _labelsQueryBusinessLogic = labelsQueryBusinessLogic;
        _tracksQueryBusinessLogic = tracksQueryBusinessLogic;
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<AllMusicResponse> GetAllMusicAsync(CancellationToken cancellationToken)
    {
        IEnumerable<AlbumResponse> albums = await _albumsQueryBusinessLogic.Get(PaginationQuery.Empty, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<GenreResponse> genres = await _genresQueryBusinessLogic.Get(PaginationQuery.Empty, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<KeyResponse> keys = await _keysQueryBusinessLogic.Get(PaginationQuery.Empty, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<LabelResponse> labels = await _labelsQueryBusinessLogic.Get(PaginationQuery.Empty, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<TrackResponse> tracks = await _tracksQueryBusinessLogic.Get(PaginationQuery.Empty, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);

        ConcurrentBag<ArtistAlbumsResponse> artistAlbumsResponses = [];
        ISortStrategy sortStrategy = new DefaultSortStrategy();

        IEnumerable<FlattenedAlbum> flattenedAlbums = CompiledExpressions.FlattenAlbums(albums, labels, genres);
        IEnumerable<FlattenedTrack> flattenedTracks = CompiledExpressions.FlattenedTracks(tracks, genres, keys);

        await foreach (ArtistResponse artist in _artistsQueryBusinessLogic.Get(PaginationQuery.Empty, cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<ArtistAlbumResponse> artistAlbums = [];
            flattenedAlbums.FastForEach(album => album.ArtistId == artist.ArtistId, album =>
            {
                ArtistAlbumResponse artistAlbum = new()
                {
                    Name = album.Name,
                    Genre = album.GenreName,
                    Label = album.LabelName,
                    ReleaseDate = album.ReleaseDate
                };
                IEnumerable<AlbumTrackResponse> albumTracks = CompiledExpressions.CreateAlbumTrackResponses(album, flattenedTracks);
                artistAlbum.AddTracks(albumTracks, sortStrategy);
                artistAlbums.Add(artistAlbum);
            });

            ArtistAlbumsResponse artistAlbumsResponse = new() { Name = artist.Name };
            artistAlbumsResponse.AddAlbums(artistAlbums, sortStrategy);
            artistAlbumsResponses.Add(artistAlbumsResponse);
        }

        AllMusicResponse allMusicResponse = new();
        allMusicResponse.AddArtists(artistAlbumsResponses, sortStrategy);
        return allMusicResponse;
    }

    #endregion
}