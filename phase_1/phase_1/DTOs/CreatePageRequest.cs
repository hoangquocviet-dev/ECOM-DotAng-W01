using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs
{
    public class CreatePageRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
