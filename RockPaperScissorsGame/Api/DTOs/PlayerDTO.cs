using System.ComponentModel.DataAnnotations;

namespace RockPaperScissorsGame.Api.DTOs
{
    public class PlayerDTO
    {
        [Required(ErrorMessage = "Player name is required.")]
        [StringLength(50, ErrorMessage = "Player name must be between 2 and 50 characters.", MinimumLength = 2)]
        public string? Name { get; set; }
    }
}
