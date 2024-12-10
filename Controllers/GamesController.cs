using Microsoft.AspNetCore.Mvc;
using APIRockPaperScissors.Models;

namespace APIRockPaperScissors.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private readonly GameService _gameService;

        public GamesController(GameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Starts a new game with Player One's name and password.
        /// </summary>
        [HttpPost("start")]
        public IActionResult StartGame([FromBody] PlayerCredentials request)
        {
            if (string.IsNullOrWhiteSpace(request.PlayerName) || string.IsNullOrWhiteSpace(request.Password) || request.Password == "string")
            {
                return BadRequest(new { message = "Player name and a secure, non-default password are required to start a game." });
            }

            try
            {
                var game = _gameService.CreateGame(request.PlayerName, request.Password);

                return CreatedAtAction(nameof(GetGameStatus), new { id = game.Id }, new { PlayerOneName = game.PlayerOneName, GameId = game.Id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Joins an existing game as Player Two.
        /// </summary>
        [HttpPost("{id}/join")]
        public IActionResult JoinGame(string id, [FromBody] PlayerCredentials request)
        {
            if (string.IsNullOrWhiteSpace(request.PlayerName) || string.IsNullOrWhiteSpace(request.Password) || request.Password == "string")
            {
                return BadRequest(new { message = "Player name and non-default password are required to join a game." });
            }

            try
            {
                var game = _gameService.JoinGame(id, request.PlayerName, request.Password);

                return Ok(new { message = $"The game is ready with the players.", PlayerOneName = game.PlayerOneName, PlayerTwoName = game.PlayerTwoName});
            }
            catch (Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Submits a move for a player in an existing game.
        /// Type a move between: 1 = Rock, 2 = Paper, 3 = Scissors.
        /// </summary>
        [HttpPost("{id}/move")]
        public IActionResult SubmitMove(string id, [FromBody] PlayerMove request)
        {
            if (request.Credentials == null || string.IsNullOrWhiteSpace(request.Credentials.Password) || request.Credentials.Password == "string")
            {
                return BadRequest(new { message = "A secure, non-default password is required to submit a move." });
            }

            try
            {
                var game = _gameService.SubmitMove(id, request.Credentials, request.Move);
                return Ok(new { message = $"Move submitted successfully by {request.Credentials.PlayerName}", gameStatus = game.IsCompleted ? "Game Completed" : "In Progress" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        /// <summary>
        /// Retrieves the current status of the game, including moves and results.
        /// </summary>
        [HttpGet("{id}/status")]
        public IActionResult GetGameStatus(string id)
        {
            try
            {
                var game = _gameService.GetGameStatus(id);

                return Ok(new
                {
                    PlayerOneName = game.PlayerOneName,
                    PlayerTwoName = game.PlayerTwoName,
                    PlayerOneMove = game.IsCompleted ? game.PlayerOneMove : "Hidden",
                    PlayerTwoMove = game.IsCompleted ? game.PlayerTwoMove : "Hidden",
                    Result = game.ResultDescription
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Game not found" });
            }
        }

        /// <summary>
        /// Retrieves the current status of the game.
        /// </summary>
        [HttpGet("{id}/spectate")]
        public IActionResult GetStatusAsSpectator(string id)
        {
            try
            {
                var game = _gameService.Spectate(id);

                return Ok(new
                {
                    PlayerOneName = game.PlayerOneName,
                    PlayerTwoName = game.PlayerTwoName,
                    Result = game.ResultDescription
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Game not found" });
            }
        }
    }
}
