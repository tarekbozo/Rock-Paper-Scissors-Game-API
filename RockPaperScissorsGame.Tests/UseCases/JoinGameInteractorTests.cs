using Moq;
using RockPaperScissorsGame.Core.UseCases;
using RockPaperScissorsGame.Core.Interfaces;
using RockPaperScissorsGame.Core.Entities;
using RockPaperScissorsGame.Core.Exceptions;

namespace RockPaperScissorsGame.Tests.UseCases
{
    public class JoinGameInteractorTests
    {
        private readonly Mock<IGameRepository> _mockRepository;
        private readonly JoinGameInteractor _interactor;

        public JoinGameInteractorTests()
        {
            _mockRepository = new Mock<IGameRepository>();
            _interactor = new JoinGameInteractor(_mockRepository.Object);
        }

        [Fact]
        public void Execute_ShouldAddPlayer2_WhenGameExistsAndNotFull()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var player1 = new Player { Name = "Player1" };
            var newPlayerName = "Player2";

            var existingGame = new Game
            {
                Id = gameId,
                Player1 = player1,
                Status = "Created"
            };

            _mockRepository.Setup(repo => repo.GetById(gameId)).Returns(existingGame);
            _mockRepository.Setup(repo => repo.Save(It.IsAny<Game>()));

            // Act
            var updatedGame = _interactor.Execute(gameId, newPlayerName);

            // Assert
            Assert.NotNull(updatedGame);
            Assert.Equal("Joined", updatedGame.Status);
            Assert.Equal("Player1", updatedGame.Player1?.Name);
            Assert.Equal("Player2", updatedGame.Player2?.Name);
            Assert.Null(updatedGame.Winner); // Winner should remain null

            // Verify Save was called
            _mockRepository.Verify(repo => repo.Save(existingGame), Times.Once);
        }

        [Fact]
        public void Execute_ShouldThrowGameNotFoundException_WhenGameDoesNotExist()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var playerName = "Player2";

            _mockRepository
                 .Setup(repo => repo.GetById(It.IsAny<Guid>()))
                 .Returns<Game>(valueFunction: null); 


            // Act & Assert
            Assert.Throws<GameNotFoundException>(() => _interactor.Execute(gameId, playerName));

            // Verify Save was never called
            _mockRepository.Verify(repo => repo.Save(It.IsAny<Game>()), Times.Never);
        }

        [Fact]
        public void Execute_ShouldThrowInvalidOperationException_WhenGameIsFull()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var existingGame = new Game
            {
                Id = gameId,
                Player1 = new Player { Name = "Player1" },
                Player2 = new Player { Name = "Player2" },
                Status = "Joined"
            };

            _mockRepository.Setup(repo => repo.GetById(gameId)).Returns(existingGame);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _interactor.Execute(gameId, "Player3"));

            // Verify Save was never called
            _mockRepository.Verify(repo => repo.Save(It.IsAny<Game>()), Times.Never);
        }

        [Fact]
        public void Execute_ShouldThrowInvalidOperationException_WhenPlayerNameAlreadyExists()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                Id = gameId,
                Status = "Created", // Required status to avoid CS9035
                Player1 = new Player { Name = "Player1" }
            };

            // Mock the repository to return the game
            _mockRepository
                .Setup(repo => repo.GetById(gameId))
                .Returns(game); // No Task.FromResult needed since GetById is synchronous

            var interactor = new JoinGameInteractor(_mockRepository.Object);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                interactor.Execute(gameId, "Player1"));

            Assert.Equal("A player with this name has already joined the game. Please use a unique name.", exception.Message);
        }



    }
}
