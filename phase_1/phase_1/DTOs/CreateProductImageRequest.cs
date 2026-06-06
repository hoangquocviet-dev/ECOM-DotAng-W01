namespace phase_1.DTOs
{
    public class CreateProductImageRequest
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
