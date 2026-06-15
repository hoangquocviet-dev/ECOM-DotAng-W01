using System.Text.Json.Serialization;

namespace phase_1.Models
{
    public class ReviewMedia
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        
        [JsonIgnore]
        public Review? Review { get; set; }

        public string MediaUrl { get; set; } = string.Empty;
        public string MediaType { get; set; } = "image"; 
    }
}
