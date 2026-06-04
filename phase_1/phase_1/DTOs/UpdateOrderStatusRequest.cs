using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs
{
    public class UpdateOrderStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
