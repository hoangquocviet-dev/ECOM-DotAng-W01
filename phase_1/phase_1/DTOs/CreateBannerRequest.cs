using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs
{
    public class CreateBannerRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        public string TargetUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int OrderIndex { get; set; } = 0;
    }
}
