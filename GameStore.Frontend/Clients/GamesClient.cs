using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GamesClient
{
    private readonly List<GameSummary> _games =
    [
        new()
        {
            Id = 1,
            Name = "Street Fighter II",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateOnly(1992, 7, 15)
        },
        new()
        {
            Id = 2,
            Name = "Final Fantasy XIV",
            Genre = "RolePlaying",
            Price = 59.99M,
            ReleaseDate = new DateOnly(2010, 9, 30)
        },
        new()
        {
            Id = 3,
            Name = "FIFA 23",
            Genre = "Sports",
            Price = 69.99M,
            ReleaseDate = new DateOnly(2022, 9, 27)
        }
    ];

    private readonly Genre[] _genres = new GenresClient().GetGenres();
    
    public GameSummary[] GetGames() =>_games.ToArray();

    public void AddGame(GameDetails game)
    {
        if(game.GenreId is null) throw new ArgumentNullException(nameof(game.GenreId));
        var genre = _genres.Single(genre => genre.Id == int.Parse(game.GenreId));

        var gameSummary = new GameSummary()
        {
            Id = _games.Count + 1,
            Name = game.Name,
            Genre = genre.Name,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
        
        _games.Add(gameSummary);
    }
}