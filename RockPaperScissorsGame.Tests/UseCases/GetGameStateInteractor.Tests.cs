using Moq;
using RockPaperScissorsGame.Core.Entities;
using RockPaperScissorsGame.Core.Exceptions;
using RockPaperScissorsGame.Core.Interfaces;
using RockPaperScissorsGame.Core.UseCases;
using System;
using Xunit;

namespace RockPaperScissorsGame.Tests.UseCases
{
    public class GetGameStateInteractorTests
    {
        private readonly Mock<IGameRepository> _mockRepository;
        private readonly GetGameStateInteractor _interactor;

        public GetGameStateInteractorTests()
        {
            _mockRepository = new Mock<IGameRepository>();
            _interactor = new GetGameStateInteractor(_mockRepository.Object);
        }

        [Fact]
        public void Execute_ShouldReturnGame_WhenGameExists()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var expectedGame = new Game
            {
                Id = gameId,
                Status = "InProgress",
                Player1 = new Player { Name = "Player1" },
                Player2 = new Player { Name = "Player2" }
            };

            _mockRepository.Setup(repo => repo.GetById(gameId)).Returns(expectedGame);

            // Act
            var result = _interactor.Execute(gameId);

            // Assert
            Assert.Equal(expectedGame, result);
            _mockRepository.Verify(repo => repo.GetById(gameId), Times.Once);
        }

        [Fact]
        public void Execute_ShouldThrowGameNotFoundException_WhenGameDoesNotExist()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetById(gameId)).Returns((Game)null);

            // Act & Assert
            var exception = Assert.Throws<GameNotFoundException>(() => _interactor.Execute(gameId));
            Assert.Equal($"Game with ID {gameId} not found.", exception.Message);

            _mockRepository.Verify(repo => repo.GetById(gameId), Times.Once);
        }

        [Fact]
        public void Execute_ShouldCallRepositoryOnce()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var expectedGame = new Game
            {
                Id = gameId,
                Status = "InProgress",
                Player1 = new Player { Name = "Player1" }
            };

            _mockRepository.Setup(repo => repo.GetById(gameId)).Returns(expectedGame);

            // Act
            var result = _interactor.Execute(gameId);

            // Assert
            Assert.NotNull(result);
            _mockRepository.Verify(repo => repo.GetById(gameId), Times.Once);
        }
    }
}
