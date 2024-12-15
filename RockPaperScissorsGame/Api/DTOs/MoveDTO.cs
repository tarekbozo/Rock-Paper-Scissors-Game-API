using RockPaperScissorsGame.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace RockPaperScissorsGame.Api.DTOs
{
    public class MoveDTO
    {
        [Required(ErrorMessage = "Player name is required.")]
        [StringLength(50, ErrorMessage = "Player name must be between 2 and 50 characters.", MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Move is required.")]
        public string Move { get; set; } = string.Empty; // Accepts 'Rock', 'Paper', 'Scissors', or 0, 1, 2
    }
}
