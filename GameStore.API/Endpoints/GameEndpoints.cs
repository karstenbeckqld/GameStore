using System;
using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Endpoints;

public static class GameEndpoints
{
    private const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> Games =
    [
        new GameDto(
            1,
            "Street Fighter II",
            "Fighting",
            19.99m,
            new DateOnly(1992, 7, 15)),
        new GameDto(
            2,
            "Final Fantasy XIV",
            "Roleplaying",
            59.99m,
            new DateOnly(2010, 9, 30)),
        new GameDto(
            3,
            "FIFA 23",
            "Sports",
            69.99m,
            new DateOnly(2022, 9, 27))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        // GET /Games
        // Returns a list of Games
        app.MapGet("games", (GameStoreContext dbContext) =>
        {
            var gamesList = new List<GameDto>();

            var games = dbContext.Games.ToListAsync().Result;
            
           
            foreach (var game in games)
            {
                GameDto gameDto = new(
                    game.Id,
                    game.Name,
                    game.Genre!.Name,
                    game.Price,
                    game.ReleaseDate
                );
                
                gamesList.Add(gameDto);
            }


            return games;
        });

        // GET /Games/{id}
        // Returns a game by ID
        app.MapGet("games/{id:int}", (int id) =>
            {
                GameDto? game = Games.Find(game => game.Id == id);

                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndpointName);

        // POST /Games
        // Creates a new game
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                Genre = dbContext.Genres.Find(newGame.GenreId),
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate,
            };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            GameDto gameDto = new(
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, gameDto);
        });

        // PUT /Games/{id}
        // Updates an existing game
        group.MapPut("/{id:int}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = Games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            Games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE /Games/{id}
        // Deletes a game by ID
        group.MapDelete("/{id:int}", (int id) =>
        {
            Games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}