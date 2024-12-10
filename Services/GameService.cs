using APIRockPaperScissors.Models;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

public class GameService
{
    private readonly ConcurrentDictionary<string, Game> _games = new();

    /// <summary>
    /// Validates if a player's name contains only alphabetic characters and spaces.
    /// </summary>
    private bool IsValidPlayerName(string playerName)
    {
        // Regex: Allows only letters (A-Z, a-z) and spaces
        return Regex.IsMatch(playerName, @"^[a-zA-Z\s]+$");
    }

    /// <summary>
    /// Creates a new game with Player One's name and password.
    /// </summary>
    public Game CreateGame(string playerName, string password)
    {
        if (string.IsNullOrWhiteSpace(playerName) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Player name and password are required to start a game.");

        if (!IsValidPlayerName(playerName))
            throw new ArgumentException("Player name must only contain alphabetic characters and spaces.");

        var game = new Game { PlayerOneName = playerName, PlayerOnePassword = password };
        _games[game.Id] = game;

        return game;
    }

    /// <summary>
    /// Joins an existing game as Player Two, with name and password.
    /// </summary>
    public Game JoinGame(string gameId, string playerName, string password)
    {
        if (!_games.TryGetValue(gameId, out var game))
            throw new KeyNotFoundException("Game not found");

        if (!string.IsNullOrEmpty(game.PlayerTwoName))
            throw new InvalidOperationException("Game already has two players");

        if (string.IsNullOrWhiteSpace(playerName) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Player name and password are required to join a game.");

        if (!IsValidPlayerName(playerName))
            throw new ArgumentException("Player name must only contain alphabetic characters and spaces.");

        game.PlayerTwoName = playerName;
        game.PlayerTwoPassword = password;

        return game;
    }

    /// <summary>
    /// Submits a move for a player, verifying with name and password.
    /// </summary>
    public Game SubmitMove(string gameId, PlayerCredentials credentials, int move)
    {
        if (!_games.TryGetValue(gameId, out var game))
            throw new KeyNotFoundException("Game not found");

        // Check if the game is already completed
        if (game.IsCompleted)
            throw new InvalidOperationException("This game is already completed. No further moves can be submitted.");

        var moveString = MoveToString(move);

        // Verify the player and password before accepting the move
        if (game.PlayerOneName == credentials.PlayerName && game.PlayerOnePassword == credentials.Password)
        {
            game.PlayerOneMove = moveString;
        }
        else if (game.PlayerTwoName == credentials.PlayerName && game.PlayerTwoPassword == credentials.Password)
        {
            game.PlayerTwoMove = moveString;
        }
        else
        {
            throw new InvalidOperationException("Invalid player name or password.");
        }

        // Determine the result once both moves are submitted
        if (!string.IsNullOrEmpty(game.PlayerOneMove) && !string.IsNullOrEmpty(game.PlayerTwoMove))
            game.ResultDescription = DetermineResult(game.PlayerOneMove, game.PlayerTwoMove, game.PlayerOneName, game.PlayerTwoName);
        

        return game;
    }


    /// <summary>
    /// Retrieves the current status of the game.
    /// </summary>
    public Game GetGameStatus(string gameId)
    {
        if (_games.TryGetValue(gameId, out var game))
        {
            return game;
        }

        throw new KeyNotFoundException("Game not found");
    }

    /// <summary>
    /// Converts an integer move to its string representation (Rock, Paper, Scissors).
    /// </summary>
    private string MoveToString(int move) => move switch
    {
        1 => "Rock",
        2 => "Paper",
        3 => "Scissors",
        _ => throw new ArgumentException("Invalid move. Pick between: 1 => Rock, 2 => Paper, 3 => Scissors")
    };

    /// <summary>
    /// Determines the result of the game based on the moves of Player One and Player Two.
    /// </summary>
    private string DetermineResult(string playerOneMove, string playerTwoMove, string playerOneName, string playerTwoName)
    {
        if (playerOneMove == playerTwoMove)
            return "It's a Tie!";

        return (playerOneMove, playerTwoMove) switch
        {
            ("Rock", "Scissors") => $"{playerOneName} Wins!",
            ("Scissors", "Paper") => $"{playerOneName} Wins!",
            ("Paper", "Rock") => $"{playerOneName} Wins!",
            _ => $"{playerTwoName} Wins!"
        };
    }
    /// <summary>
    /// Spectate the result of the game.
    /// </summary>
    public Game Spectate(string gameId)
    {
        if (_games.TryGetValue(gameId, out var game))
            return game;

        throw new KeyNotFoundException("Game not found");
    }
}
