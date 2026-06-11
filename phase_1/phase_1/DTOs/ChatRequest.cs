using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs
{
    public class ChatRequest
    {
        [Required]
        public string Message { get; set; } = string.Empty;
    }
}
