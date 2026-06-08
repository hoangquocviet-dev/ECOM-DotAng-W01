using System;
using System.Text.Json.Serialization;

namespace phase_1.Models
{
    public class ReturnRequest
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }
        
        public int UserId { get; set; }
        [JsonIgnore]
        public Users? User { get; set; }

        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedDate { get; set; }
        public string AdminNote { get; set; } = string.Empty;
    }
}
