namespace RockPaperScissorsGame.Api.DTOs
{
    public class GameDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = "Created"; // Status: Created, Joined, Completed
        public PlayerDetailsDTO? Player1 { get; set; }
        public PlayerDetailsDTO? Player2 { get; set; }
        public string? Winner { get; set; }
    }

    public class PlayerDetailsDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Move { get; set; } // Nullable if the move hasn't been made yet
    }

}
