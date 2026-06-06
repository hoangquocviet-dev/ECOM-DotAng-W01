namespace phase_1.DTOs
{
    public class CheckoutRequest
    {
        public string ShippingAddress { get; set; } = string.Empty;
        public string? VoucherCode { get; set; }
        public string PaymentMethod { get; set; } = "COD";
    }
}
