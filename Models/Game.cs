namespace APIRockPaperScissors.Models
{
    /// <summary>
    /// Represents a game of Rock-Paper-Scissors, including players, moves, and the result.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Unique identifier for the game.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Name of the first player in the game.
        /// </summary>
        public string PlayerOneName { get; set; }

        /// <summary>
        /// Name of the second player in the game.
        /// </summary>
        public string PlayerTwoName { get; set; }

        /// <summary>
        /// Move chosen by the first player (Rock, Paper, or Scissors).
        /// </summary>
        public string PlayerOneMove { get; set; }

        /// <summary>
        /// Move chosen by the second player (Rock, Paper, or Scissors).
        /// </summary>
        public string PlayerTwoMove { get; set; }

        /// <summary>
        /// The result of the game, indicating the winner or if it's a tie.
        /// </summary>
        public string ResultDescription { get; set; } = GameResult.InProgress.ToString();

        /// <summary>
        /// Indicates whether the game is complete (true if there's a result, false if still in progress).
        /// </summary>
        public bool IsCompleted => ResultDescription != GameResult.InProgress.ToString();

        // Passwords for each player (should not be shown in responses)
        public string PlayerOnePassword { get; set; }
        public string PlayerTwoPassword { get; set; }
    }
}




public enum Move
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}


public enum GameResult
{
    InProgress,
    Player1Wins,
    Player2Wins,
    Tie
}


