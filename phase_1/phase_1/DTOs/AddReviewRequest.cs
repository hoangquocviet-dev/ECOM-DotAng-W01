using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs
{
    public class AddReviewRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public System.Collections.Generic.List<string> MediaUrls { get; set; } = new System.Collections.Generic.List<string>();
    }
}
