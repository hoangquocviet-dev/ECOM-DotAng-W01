using System.Text.Json.Serialization;

namespace phase_1.Models
{
    public class ComboItem
    {
        public int Id { get; set; }
        
        public int ComboId { get; set; }
        [JsonIgnore]
        public Combo? Combo { get; set; }

        public int ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
