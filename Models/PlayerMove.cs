namespace APIRockPaperScissors.Models
{
    /// <summary>
    /// Represents a player's move, including their credentials.
    /// Move: 1 = Rock, 2 = Paper, 3 = Scissors
    /// </summary>
    public class PlayerMove
    {
        public PlayerCredentials Credentials { get; set; }
        public Move Move { get; set; } // 1 = Rock, 2 = Paper, 3 = Scissors
    }
}
