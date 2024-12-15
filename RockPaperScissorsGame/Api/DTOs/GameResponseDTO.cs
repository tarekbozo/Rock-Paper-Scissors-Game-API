namespace RockPaperScissorsGame.Api.DTOs
{
    public class GameResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = "Created"; // Status: Created, Joined, Completed
        public PlayerDetailsDTO? Player1 { get; set; }
        public PlayerDetailsDTO? Player2 { get; set; }
        public string? Winner { get; set; }
    }
}
