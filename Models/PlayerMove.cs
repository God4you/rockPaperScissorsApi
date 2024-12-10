namespace APIRockPaperScissors.Models
{
    /// <summary>
    /// Represents a player's move, including their credentials.
    /// </summary>
    public class PlayerMove
    {
        public PlayerCredentials Credentials { get; set; }
        public int Move { get; set; } // 1 = Rock, 2 = Paper, 3 = Scissors
    }
}
