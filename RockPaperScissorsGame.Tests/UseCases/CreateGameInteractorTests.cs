using Xunit;
using Moq;
using RockPaperScissorsGame.Core.UseCases;
using RockPaperScissorsGame.Core.Interfaces;
using RockPaperScissorsGame.Core.Entities;

namespace RockPaperScissorsGame.Tests.UseCases
{
    public class CreateGameInteractorTests
    {
        [Fact]
        public void Execute_ShouldCreateGame_WithValidPlayerName()
        {
            // Arrange
            var mockGameRepository = new Mock<IGameRepository>();
            var playerName = "Player1";

            // The repository should accept a game but doesn't need to return anything
            mockGameRepository.Setup(repo => repo.Save(It.IsAny<Game>()));

            var interactor = new CreateGameInteractor(mockGameRepository.Object);

            // Act
            var game = interactor.Execute(playerName);

            // Assert
            Assert.NotNull(game); // Game should not be null
            Assert.Equal(playerName, game.Player1?.Name); // Player1 name should match
            Assert.Null(game.Player2); // Player2 should initially be null
            Assert.Equal("Created", game.Status); // Status should be 'Created'
            Assert.Null(game.Winner); // Winner should be null
            Assert.NotEqual(Guid.Empty, game.Id); // ID should not be an empty GUID

            // Verify that the Save method was called once with the correct game
            mockGameRepository.Verify(repo => repo.Save(It.IsAny<Game>()), Times.Once);
        }

        [Fact]
        public void Execute_ShouldThrowArgumentException_WhenPlayerNameIsEmpty()
        {
            // Arrange
            var mockGameRepository = new Mock<IGameRepository>();
            var interactor = new CreateGameInteractor(mockGameRepository.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => interactor.Execute(string.Empty));
            Assert.Throws<ArgumentException>(() => interactor.Execute(" "));

            // Verify that the repository's Save method was never called
            mockGameRepository.Verify(repo => repo.Save(It.IsAny<Game>()), Times.Never);
        }
    }
}