## Rock-Paper-Scissors Game

A simple Rock-Paper-Scissors game API built in C# and .NET Core.

This project provides a REST API for two players to play a Rock-Paper-Scissors game session. 
The API allows two players to start, join, and play a game with secure credentials. No persistence mechanism is used, ensuring all data resides in memory.

## Installation

Using Visual Studio:
 - Open the APIRockPaperScissors.sln file in Visual Studio.
 - Ensure you have the required .NET SDK installed.
 - Build the solution
 - Start by pressing F5
 - Test/Play with swagger
	
## Requirements

.NET Core SDK 8.0 or later

## Features

- Start a Game: Player One creates a game with their name and password.
- Join a Game: Player Two joins using the game ID, their name, and password.
- Submit a Move: Both players submit their moves (1 = Rock, 2 = Paper, or 3 = Scissors).
- Game Result: Retrieve the game status to see the result (if completed) and the winner or if it's a tie.

## Rules

Player Names:
 - Must only contain alphabetic characters and spaces.
 - Numbers or special characters are not allowed.
Password:
 - Must be non-default (not "string") and non-empty.
Game Rules:
 - The game is completed once both players submit their moves.
 - A tie results in "It's a Tie!" and marks the game as completed.
Data Storage:
 - The game state is stored in memory only. Restarting the server clears all game data.

## Project Structure

Controllers:
 - GamesController.cs: Handles all API endpoints.
	
Models:
 - Game.cs: Represents the game state.
 - PlayerCredentials.cs: Represents player login credentials.
 - PlayerMove.cs: Represents a player’s move.
	
Services:
 - GameService.cs: Implements the game logic, such as creating games, joining games, submitting moves, and determining results.