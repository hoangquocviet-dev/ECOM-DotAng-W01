using System;
using System.Collections.Generic;

namespace phase_1.Models
{
    public class Combo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ComboItem> ComboItems { get; set; } = new List<ComboItem>();
    }
}
