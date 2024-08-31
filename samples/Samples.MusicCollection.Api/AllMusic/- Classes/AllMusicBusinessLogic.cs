using Cognas.ApiTools.Microservices;
using Cognas.ApiTools.Pagination;
using Cognas.Tools.Shared.Extensions;
using Samples.MusicCollection.Api.Albums;
using Samples.MusicCollection.Api.AllMusic.Albums;
using Samples.MusicCollection.Api.AllMusic.Artists;
using Samples.MusicCollection.Api.AllMusic.MixableTracks;
using Samples.MusicCollection.Api.AllMusic.MixableTracks.Rules;
using Samples.MusicCollection.Api.AllMusic.Tracks;
using Samples.MusicCollection.Api.Artists;
using Samples.MusicCollection.Api.Genres;
using Samples.MusicCollection.Api.Keys;
using Samples.MusicCollection.Api.Labels;
using Samples.MusicCollection.Api.Tracks;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Runtime.InteropServices;

namespace Samples.MusicCollection.Api.AllMusic;

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
    private readonly IEnumerable<IMixableTracksRule> _mixableTrackRules;

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
    /// <param name="mixableTrackRules"></param>
    public AllMusicBusinessLogic(IQueryMicroserviceBusinessLogic<AlbumResponse> albumsQueryBusinessLogic,
                                 IQueryMicroserviceBusinessLogic<ArtistResponse> artistsQueryBusinessLogic,
                                 IQueryMicroserviceBusinessLogic<GenreResponse> genresQueryBusinessLogic,
                                 IQueryMicroserviceBusinessLogic<KeyResponse> keysQueryBusinessLogic,
                                 IQueryMicroserviceBusinessLogic<LabelResponse> labelsQueryBusinessLogic,
                                 IQueryMicroserviceBusinessLogic<TrackResponse> tracksQueryBusinessLogic,
                                 IEnumerable<IMixableTracksRule> mixableTrackRules)
    {
        ArgumentNullException.ThrowIfNull(albumsQueryBusinessLogic, nameof(albumsQueryBusinessLogic));
        ArgumentNullException.ThrowIfNull(artistsQueryBusinessLogic, nameof(artistsQueryBusinessLogic));
        ArgumentNullException.ThrowIfNull(genresQueryBusinessLogic, nameof(genresQueryBusinessLogic));
        ArgumentNullException.ThrowIfNull(keysQueryBusinessLogic, nameof(keysQueryBusinessLogic));
        ArgumentNullException.ThrowIfNull(labelsQueryBusinessLogic, nameof(labelsQueryBusinessLogic));
        ArgumentNullException.ThrowIfNull(tracksQueryBusinessLogic, nameof(tracksQueryBusinessLogic));
        ArgumentNullException.ThrowIfNull(mixableTrackRules, nameof(mixableTrackRules));

        _albumsQueryBusinessLogic = albumsQueryBusinessLogic;
        _artistsQueryBusinessLogic = artistsQueryBusinessLogic;
        _genresQueryBusinessLogic = genresQueryBusinessLogic;
        _keysQueryBusinessLogic = keysQueryBusinessLogic;
        _labelsQueryBusinessLogic = labelsQueryBusinessLogic;
        _tracksQueryBusinessLogic = tracksQueryBusinessLogic;
        _mixableTrackRules = mixableTrackRules;
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
        IEnumerable<GenreResponse> genres = await _genresQueryBusinessLogic.Get(PaginationQuery.Empty, cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<FlattenedAlbum> flattenedAlbums = await GetFlattenedAlbumsAsync(genres, cancellationToken).ConfigureAwait(false);
        IEnumerable<FlattenedTrack> flattenedTracks = await GetFlattenedTracksAsync(genres, cancellationToken).ConfigureAwait(false);
        ConcurrentBag<ArtistAlbumsResponse> artistAlbumsResponses = [];
        ISortStrategy sortStrategy = new DefaultSortStrategy();

        await foreach (ArtistResponse artist in _artistsQueryBusinessLogic.Get(PaginationQuery.Empty, cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<ArtistAlbumResponse> artistAlbums = [];
            flattenedAlbums.FastForEach(album => album.ArtistId == artist.ArtistId, album =>
            {
                ArtistAlbumResponse artistAlbum = CreateArtistAlbumResponse(album);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mixableTrackRequests"></param>
    /// <returns></returns>
    public IEnumerable<MixableTrackResponse> AreMixableTracks(IEnumerable<MixableTrackRequest> mixableTrackRequests)
    {
        ReadOnlySpan<MixableTrackRequest> mixableTrackRequestsSpan = CollectionsMarshal.AsSpan(mixableTrackRequests.ToList());
        int length = mixableTrackRequestsSpan.Length;

        HashSet<MixableTrackResponse> mixableTrackResponses = [];

        for (int trackAIndex = 0; trackAIndex < length; trackAIndex++)
        {
            for (int trackBIndex = trackAIndex + 1; trackBIndex < length; trackBIndex++)
            {
                bool isMixable = true;
                MixableTrackRequest trackA = mixableTrackRequestsSpan[trackAIndex];
                MixableTrackRequest trackB = mixableTrackRequestsSpan[trackBIndex];

                _mixableTrackRules.FastForEach(rule =>
                {
                    isMixable = isMixable && rule.IsMixable(trackA, trackB);
                    if (!isMixable)
                    {
                        MixableTrackResponse notMixableResponse = CreateMixableTrackRequest(trackA, trackB, false);
                        mixableTrackResponses.Add(notMixableResponse);
                        return;
                    }
                });
                if (isMixable)
                {
                    MixableTrackResponse mixableResponse = CreateMixableTrackRequest(trackA, trackB, true);
                    mixableTrackResponses.Add(mixableResponse);
                }
            }
        }

        return mixableTrackResponses;
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="genres"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<IEnumerable<FlattenedAlbum>> GetFlattenedAlbumsAsync(IEnumerable<GenreResponse> genres, CancellationToken cancellationToken)
    {
        IEnumerable<AlbumResponse> albums = await _albumsQueryBusinessLogic.Get(PaginationQuery.Empty, cancellationToken).ToFrozenSetAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<LabelResponse> labels = await _labelsQueryBusinessLogic.Get(PaginationQuery.Empty, cancellationToken).ToFrozenSetAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<FlattenedAlbum> flattenedAlbums = CompiledExpressions.FlattenAlbums(albums, labels, genres);
        return flattenedAlbums;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="genres"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<IEnumerable<FlattenedTrack>> GetFlattenedTracksAsync(IEnumerable<GenreResponse> genres, CancellationToken cancellationToken)
    {
        IEnumerable<KeyResponse> keys = await _keysQueryBusinessLogic.Get(PaginationQuery.Empty, cancellationToken).ToFrozenSetAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<TrackResponse> tracks = await _tracksQueryBusinessLogic.Get(PaginationQuery.Empty, cancellationToken).ToFrozenSetAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<FlattenedTrack> flattenedTracks = CompiledExpressions.FlattenedTracks(tracks, genres, keys).ToFrozenSet();
        return flattenedTracks;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="flattenedAlbum"></param>
    /// <returns></returns>
    private static ArtistAlbumResponse CreateArtistAlbumResponse(FlattenedAlbum flattenedAlbum) => new()
    {
        Name = flattenedAlbum.Name,
        Genre = flattenedAlbum.GenreName,
        Label = flattenedAlbum.LabelName,
        ReleaseDate = flattenedAlbum.ReleaseDate
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trackA"></param>
    /// <param name="trackB"></param>
    /// <param name="isMixable"></param>
    /// <returns></returns>
    private static MixableTrackResponse CreateMixableTrackRequest(MixableTrackRequest trackA, MixableTrackRequest trackB, bool isMixable) => new()
    {
        TrackAId = trackA.TrackId,
        TrackBId = trackB.TrackId,
        IsMixable = isMixable
    };

    #endregion
}