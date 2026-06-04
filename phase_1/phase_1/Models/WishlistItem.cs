using System;
using System.Text.Json.Serialization;

namespace phase_1.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        [JsonIgnore]
        public Users? User { get; set; }

        public int ProductId { get; set; }
        
        public Product? Product { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}
