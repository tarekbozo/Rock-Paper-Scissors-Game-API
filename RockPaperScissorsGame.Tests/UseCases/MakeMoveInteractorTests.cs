using Moq;
using RockPaperScissorsGame.Core.Entities;
using RockPaperScissorsGame.Core.Exceptions;
using RockPaperScissorsGame.Core.Interfaces;
using RockPaperScissorsGame.Core.UseCases;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RockPaperScissorsGame.Tests.UseCases
{
    public class MakeMoveInteractorTests
    {
        private readonly Mock<IGameRepository> _mockRepository;
        private readonly MakeMoveInteractor _interactor;

        public MakeMoveInteractorTests()
        {
            _mockRepository = new Mock<IGameRepository>();
            _interactor = new MakeMoveInteractor(_mockRepository.Object);
        }

        [Fact]
        public async Task Execute_ShouldThrowException_WhenGameNotFound()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetById(gameId)).Returns((Game)null);

            // Act & Assert
            await Assert.ThrowsAsync<GameNotFoundException>(() =>
                _interactor.Execute(gameId, "Player1", "Rock"));
        }

        [Fact]
        public async Task Execute_ShouldThrowException_WhenPlayerNameIsInvalid()
        {
            // Arrange
            var gameId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _interactor.Execute(gameId, "", "Rock"));
        }

        [Fact]
        public async Task Execute_ShouldThrowException_WhenPlayerNotPartOfGame()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                Id = gameId,
                Status = "InProgress", 
                Player1 = new Player { Name = "Player1" }
            };

            _mockRepository.Setup(repo => repo.GetById(gameId)).Returns(game);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(() =>
                _interactor.Execute(gameId, "UnknownPlayer", "Rock"));
        }

        [Fact]
        public async Task Execute_ShouldThrowException_WhenPlayerAlreadyMadeMove()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                Id = gameId,
                Status = "InProgress", 
                Player1 = new Player { Name = "Player1", CurrentMove = Move.Rock }
            };

            _mockRepository.Setup(repo => repo.GetById(gameId)).Returns(game);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _interactor.Execute(gameId, "Player1", "Scissors"));
        }

        [Fact]
        public async Task Execute_ShouldAssignMove_WhenPlayerMakesValidMove()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                Id = gameId,
                Status = "InProgress", 
                Player1 = new Player { Name = "Player1" },
                Player2 = new Player { Name = "Player2" }
            };

            _mockRepository.Setup(repo => repo.GetById(gameId)).Returns(game);

            // Act
            var result = await _interactor.Execute(gameId, "Player1", "Rock");

            // Assert
            Assert.Equal(Move.Rock, result.Player1.CurrentMove);
            _mockRepository.Verify(repo => repo.Save(game), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldDetermineWinner_WhenBothPlayersHaveMadeMoves()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                Id = gameId,
                Status = "InProgress", 
                Player1 = new Player { Name = "Player1" },
                Player2 = new Player { Name = "Player2" }
            };

            _mockRepository.Setup(repo => repo.GetById(gameId)).Returns(game);

            // Player1 makes a move
            await _interactor.Execute(gameId, "Player1", "Rock");
            // Player2 makes a move
            var result = await _interactor.Execute(gameId, "Player2", "Scissors");

            // Assert
            Assert.Equal("Player1", result.Winner);
            Assert.Equal("Completed", result.Status);
            _mockRepository.Verify(repo => repo.Save(game), Times.Exactly(2));
        }

        [Fact]
        public async Task Execute_ShouldDeclareTie_WhenMovesAreTheSame()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                Id = gameId,
                Status = "InProgress", 
                Player1 = new Player { Name = "Player1" },
                Player2 = new Player { Name = "Player2" }
            };

            _mockRepository.Setup(repo => repo.GetById(gameId)).Returns(game);

            // Player1 makes a move
            await _interactor.Execute(gameId, "Player1", "Rock");
            // Player2 makes the same move
            var result = await _interactor.Execute(gameId, "Player2", "Rock");

            // Assert
            Assert.Equal("Tie", result.Winner);
            Assert.Equal("Completed", result.Status);
            _mockRepository.Verify(repo => repo.Save(game), Times.Exactly(2));
        }
    }
}
