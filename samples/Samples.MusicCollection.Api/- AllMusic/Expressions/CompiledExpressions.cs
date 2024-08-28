using Samples.MusicCollection.Api.Albums;
using Samples.MusicCollection.Api.AllMusic.Responses;
using Samples.MusicCollection.Api.Genres;
using Samples.MusicCollection.Api.Keys;
using Samples.MusicCollection.Api.Labels;
using Samples.MusicCollection.Api.Tracks;
using System.Linq.Expressions;

namespace Samples.MusicCollection.Api.AllMusic.Expressions;

/// <summary>
/// 
/// </summary>
public static class CompiledExpressions
{
    #region Field Declarations

    private static readonly Expression<Func<IEnumerable<AlbumResponse>, IEnumerable<LabelResponse>, IEnumerable<GenreResponse>, IEnumerable<FlattenedAlbum>>>
        _flattenedAlbumsExpression = (albums, labels, genres) => from album in albums
                                                                 join label in labels on album.LabelId equals label.LabelId
                                                                 from genre in genres.Where(genre => genre.GenreId == album.GenreId).DefaultIfEmpty()
                                                                 select new FlattenedAlbum()
                                                                 {
                                                                     AlbumId = album.AlbumId,
                                                                     ArtistId = album.ArtistId,
                                                                     GenreName = genre == null ? string.Empty : genre.Name,
                                                                     LabelName = label.Name,
                                                                     Name = album.Name,
                                                                     ReleaseDate = album.ReleaseDate,
                                                                 };

    private static readonly Expression<Func<IEnumerable<TrackResponse>, IEnumerable<GenreResponse>, IEnumerable<KeyResponse>, IEnumerable<FlattenedTrack>>>
        _flattenedTracksExpression = (tracks, genres, keys) => (from track in tracks
                                                                join genre in genres on track.GenreId equals genre.GenreId
                                                                from key in keys.Where(key => key.KeyId == track.KeyId).DefaultIfEmpty()
                                                                select new FlattenedTrack()
                                                                {
                                                                    AlbumId = track.AlbumId,
                                                                    Bpm = track.Bpm,
                                                                    CamelotCode = key == null ? string.Empty : key.CamelotCode,
                                                                    GenreName = genre.Name,
                                                                    KeyName = key == null ? string.Empty : key.Name,
                                                                    Name = track.Name,
                                                                    TrackNumber = track.TrackNumber
                                                                });

    private static readonly Expression<Func<FlattenedAlbum, IEnumerable<FlattenedTrack>, IEnumerable<AlbumTrackResponse>>>
        _createAlbumTrackResponsesExpression = (flattenedAlbum, flattenedTracks) => from flattenedTrack in flattenedTracks
                                                                                    where flattenedTrack.AlbumId == flattenedAlbum.AlbumId
                                                                                    select new AlbumTrackResponse()
                                                                                    {
                                                                                        TrackNumber = flattenedTrack.TrackNumber,
                                                                                        Name = flattenedTrack.Name,
                                                                                        Genre = flattenedTrack.GenreName,
                                                                                        Bpm = flattenedTrack.Bpm,
                                                                                        CamelotCode = flattenedTrack.CamelotCode,
                                                                                        Key = flattenedTrack.KeyName
                                                                                    };

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public static Func<IEnumerable<AlbumResponse>, IEnumerable<LabelResponse>, IEnumerable<GenreResponse>, IEnumerable<FlattenedAlbum>> FlattenAlbums { get; } = _flattenedAlbumsExpression.Compile();

    /// <summary>
    /// 
    /// </summary>
    public static Func<IEnumerable<TrackResponse>, IEnumerable<GenreResponse>, IEnumerable<KeyResponse>, IEnumerable<FlattenedTrack>> FlattenedTracks { get; } = _flattenedTracksExpression.Compile();

    /// <summary>
    /// 
    /// </summary>
    public static Func<FlattenedAlbum, IEnumerable<FlattenedTrack>, IEnumerable<AlbumTrackResponse>> CreateAlbumTrackResponses { get; } = _createAlbumTrackResponsesExpression.Compile();

    #endregion
}