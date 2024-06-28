using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Shared.Extensions;
using Samples.MusicCollection.Api.Albums;
using Samples.MusicCollection.Api.AllMusic.Abstractions;
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
        PaginationQuery emptyPagination = PaginationQuery.Empty;
        IAsyncEnumerable<ArtistResponse> artists = _artistsQueryBusinessLogic.Get(emptyPagination, cancellationToken);
        IEnumerable<AlbumResponse> albums = await _albumsQueryBusinessLogic.Get(emptyPagination, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<GenreResponse> genres = await _genresQueryBusinessLogic.Get(emptyPagination, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<KeyResponse> keys = await _keysQueryBusinessLogic.Get(emptyPagination, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<LabelResponse> labels = await _labelsQueryBusinessLogic.Get(emptyPagination, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<TrackResponse> tracks = await _tracksQueryBusinessLogic.Get(emptyPagination, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);

        AllMusicResponse allMusicResponse = new();
        ConcurrentBag<ArtistAlbumsResponse> artistResponses = [];
        ISortStrategy sortStrategy = new DefaultSortStrategy();

        await foreach (ArtistResponse artist in artists.ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<ArtistAlbumResponse> artistAlbums = [];
            albums.FastForEach(album => album.ArtistId == artist.ArtistId, (Action<AlbumResponse>)(album =>
            {
                ArtistAlbumResponse artistAlbum = GetArtistAlbum(album, genres, labels);
                IEnumerable<AlbumTrackResponse> albumTracks = GetAlbumTracks(album, genres, keys, tracks);
                artistAlbum.AddTracks(albumTracks, sortStrategy);
                artistAlbums.Add(artistAlbum);
            }));

            ArtistAlbumsResponse artistAlbumsResponse = CreteArtistAlbumsResponse(artist);
            artistAlbumsResponse.AddAlbums(artistAlbums, sortStrategy);
            artistResponses.Add(artistAlbumsResponse);
        }

        allMusicResponse.AddArtists(artistResponses, sortStrategy);
        return allMusicResponse;
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artist"></param>
    /// <returns></returns>
    private static ArtistAlbumsResponse CreteArtistAlbumsResponse(ArtistResponse artist)
    {
        ArtistAlbumsResponse artistResponse = new() { Name = artist.Name };
        return artistResponse;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="album"></param>
    /// <param name="genres"></param>
    /// <param name="labels"></param>
    /// <exception cref="NullReferenceException"></exception>
    private static ArtistAlbumResponse GetArtistAlbum(AlbumResponse album, IEnumerable<GenreResponse> genres, IEnumerable<LabelResponse> labels)
    {
        GenreResponse? genre = genres.FastFirstOrDefault(genre => album.GenreId == genre.GenreId);
        LabelResponse label = labels.FastFirstOrDefault(label => album.LabelId == label.LabelId) ?? throw new NullReferenceException($"{nameof(AlbumResponse)}.{nameof(LabelResponse)}");
        ArtistAlbumResponse albumResponse = new()
        {
            Name = album.Name,
            Genre = genre?.Name ?? string.Empty,
            Label = label.Name,
            ReleaseDate = album.ReleaseDate
        };
        return albumResponse;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="album"></param>
    /// <param name="genres"></param>
    /// <param name="keys"></param>
    /// <param name="tracks"></param>
    /// <returns></returns>
    private static List<AlbumTrackResponse> GetAlbumTracks(AlbumResponse album, IEnumerable<GenreResponse> genres, IEnumerable<KeyResponse> keys, IEnumerable<TrackResponse> tracks)
    {
        List<AlbumTrackResponse> trackResponses = [];
        tracks.FastForEach(track => track.AlbumId == album.AlbumId, track =>
        {
            AlbumTrackResponse trackResponse = GetAlbumTrack(genres, keys, track);
            trackResponses.Add(trackResponse);
        });
        return trackResponses;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="genres"></param>
    /// <param name="keys"></param>
    /// <param name="track"></param>
    /// <exception cref="NullReferenceException"></exception>
    private static AlbumTrackResponse GetAlbumTrack(IEnumerable<GenreResponse> genres, IEnumerable<KeyResponse> keys, TrackResponse track)
    {
        GenreResponse genre = genres.FastFirstOrDefault(genre => track.GenreId == genre.GenreId) ?? throw new NullReferenceException(nameof(TrackResponse.GenreId));
        KeyResponse? key = keys.FastFirstOrDefault(key => track.KeyId == key.KeyId);
        AlbumTrackResponse trackResponse = new()
        {
            TrackNumber = track.TrackNumber,
            Name = track.Name,
            Genre = genre.Name,
            Bpm = track.Bpm,
            CamelotCode = key?.CamelotCode,
            Key = key?.Name
        };
        return trackResponse;
    }

    #endregion
}