# Rock-Paper-Scissors Game API

This is a **REST API** for playing the classic game of **Rock, Paper, Scissors**. It allows developers to resolve disputes through a simple yet fun game. Built using **.NET 8**, it adheres to best practices for maintainability, scalability, and performance. **SignalR** is integrated for real-time updates.

---

## Features

- **Create a new game** with a unique ID.
- **Join an existing game** using the game ID.
- **Make moves** for each player (Rock, Paper, or Scissors).
- **Retrieve the current state** of the game, including players, moves, status, and winner.
- **Determine the winner** when both players have made their moves.
- **Real-time updates** via SignalR for connected clients.
- **Prevents duplicate player names** for clarity.
- **Unit Tests**: Comprehensive tests for game creation, joining, moves, and state retrieval.
- **In-memory state management** (No database; all data is lost when the server restarts).

---

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/)  
- Visual Studio 2022 or later  
- Swagger UI for testing endpoints  
- **No database required** (state is maintained in memory).

---

## Endpoints

### 1. Create a New Game  
**Endpoint:** `POST /api/games`  
**Request Body:**
```json
{
    "name": "PlayerName"
}
```

**Response:**
```json
{
    "id": "unique-game-id",
    "status": "Created",
    "player1": "PlayerName",
    "player2": null,
    "winner": null
}
```

---

### 2. Join an Existing Game  
**Endpoint:** `POST /api/games/{id}/join`  
**Request Body:**
```json
{
    "name": "PlayerName"
}
```

**Response:**
```json
{
    "id": "unique-game-id",
    "status": "Joined",
    "player1": "ExistingPlayerName",
    "player2": "PlayerName",
    "winner": null
}
```

---

### 3. Make a Move  
**Endpoint:** `POST /api/games/{id}/move`  
**Request Body:**
```json
{
    "name": "PlayerName",
    "move": "Rock"
}
```

**Response:**
```json
{
    "id": "unique-game-id",
    "status": "Completed",
    "player1": {
        "name": "Player1Name",
        "move": "Rock"
    },
    "player2": {
        "name": "Player2Name",
        "move": "Scissors"
    },
    "winner": "Player1Name"
}
```

---

### 4. Retrieve Game State  
**Endpoint:** `GET /api/games/{id}`  

**Response:**
```json
{
    "id": "unique-game-id",
    "status": "Completed",
    "player1": {
        "name": "Player1Name",
        "move": "Rock"
    },
    "player2": {
        "name": "Player2Name",
        "move": "Scissors"
    },
    "winner": "Player1Name"
}
```

---

## SignalR Integration

- **Hub Endpoint:** `/gamehub`
- Clients receive updates in real-time about the game state using SignalR.
- **Event:**  
  - `ReceiveGameUpdate`: Notifies all connected clients about changes in the game state.

---

## Setup Instructions

### Prerequisites

1. Install the latest version of [.NET 8 SDK](https://dotnet.microsoft.com/).
2. Install Visual Studio 2022 or later.

### Steps to Run

1. **Clone the repository**:
   ```bash
   git clone https://github.com/your-repo/rock-paper-scissors-game.git
   cd rock-paper-scissors-game
   ```

2. **Open the project** in Visual Studio.

3. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

4. **Build and run the project**:
   - Press `F5` in Visual Studio or run:
     ```bash
     dotnet run
     ```

5. **Swagger UI** will be available at:  
   `http://localhost:5000/swagger`

---

## Running Unit Tests

### Test Overview:
- Tests cover:
  - **Game Creation**
  - **Joining a Game**
  - **Making Moves**
  - **Determining a Winner**
  - **Game State Retrieval**

### Steps to Run Tests:
1. Open the **Test Explorer** in Visual Studio:
   - Go to `Test > Test Explorer` or press **Ctrl+E, T**.

2. Run all tests using the Test Explorer:
   - Select **"Run All"**.

3. Or run the tests from the terminal:
   ```bash
   dotnet test
   ```

---

## Example Test Results  
Here are sample unit tests and their validations:

### CreateGameInteractorTests
- Ensures a game is created with a unique ID and status.

### JoinGameInteractorTests
- Ensures that a player can join the game.
- Validates that duplicate names are not allowed.

### MakeMoveInteractorTests
- Ensures that moves are valid.
- Determines the winner when both players have made a move.
- Declares a tie if moves are the same.

### GetGameStateInteractorTests
- Ensures the game state is retrieved correctly.

---

## Notes

- **Real-time updates:** SignalR is used to notify all connected clients of any game state changes.
- **Rate Limiting Middleware**: Prevents too many requests from the same IP address.
- **Caching:** A memory cache manager can be extended for additional performance optimizations.

---

## Future Enhancements

- **Authentication**: Secure API endpoints for authenticated players.
- **Game Modes**: Add "Best of 3" or "Tournament Mode."
- **Persistent Storage**: Replace in-memory storage with a database.
- **Enhanced Real-Time Updates**: Notify only relevant clients using SignalR.

---


**Enjoy resolving your disputes with Rock, Paper, Scissors! 🎉**
