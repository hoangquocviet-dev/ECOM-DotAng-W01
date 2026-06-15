using System;
using System.Text.Json.Serialization;

namespace phase_1.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        [JsonIgnore]
        public Users? User { get; set; }

        public int ProductId { get; set; }
        
        [JsonIgnore]
        public Product? Product { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        
        public string? AdminReply { get; set; }
        public DateTime? ReplyDate { get; set; }
        
        public ICollection<ReviewMedia> ReviewMedias { get; set; } = new List<ReviewMedia>();
    }
}
