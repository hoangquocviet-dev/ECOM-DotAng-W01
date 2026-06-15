using System;

namespace phase_1.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public System.Collections.Generic.List<string> MediaUrls { get; set; } = new System.Collections.Generic.List<string>();
        public string? AdminReply { get; set; }
        public DateTime? ReplyDate { get; set; }
    }
}
