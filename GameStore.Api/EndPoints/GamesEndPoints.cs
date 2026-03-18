using GameStore.Api.Dtos;

namespace GameStore.Api.EndPoints;

public static class GamesEndPoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = new List<GameDto>
    {
        new(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
        new(2, "Astro Bot", "Platformer", 59.99m, new DateOnly(2024, 9, 6)),
        new(3, "Red Dead Redemption 2", "Action-adventure", 59.99m, new DateOnly(2018, 10, 26)),
        new(4, "The Witcher 3: Wild Hunt", "Action RPG", 39.99m, new DateOnly(2015, 5, 19)),
    };

    public static void MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/{id}
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(g => g.Id == id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        }).WithName(GetGameEndpointName);

        group.MapPost("/", (CreateGameDto gameDto) =>
        {
            
            var newGame = new GameDto(games.Count + 1, gameDto.Name, gameDto.Genre, gameDto.Price, gameDto.ReleaseDate);
            games.Add(newGame);
            return Results.CreatedAtRoute(GetGameEndpointName, new {id = newGame.Id}, newGame);
        });

        group.MapPut("/{id}", (int id, UpdateGameDto gameDto) =>
        {
            var index = games.FindIndex(g => g.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(id, gameDto.Name, gameDto.Genre, gameDto.Price, gameDto.ReleaseDate);

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id) =>
        {
            var index = games.FindIndex(g => g.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }

            games.RemoveAt(index);

            return Results.NoContent();
        });
    }
}
