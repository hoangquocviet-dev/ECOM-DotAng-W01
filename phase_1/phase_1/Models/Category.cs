using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace phase_1.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;
        public string MetaTitle { get; set; } = string.Empty;
        public string MetaDescription { get; set; } = string.Empty;
        
        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
