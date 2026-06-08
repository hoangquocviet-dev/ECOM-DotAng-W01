using System;

namespace phase_1.DTOs
{
    public class ReturnRequestDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string AdminNote { get; set; } = string.Empty;
    }

    public class CreateReturnRequest
    {
        public int OrderId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class ProcessReturnRequest
    {
        public string Status { get; set; } = string.Empty;
        public string AdminNote { get; set; } = string.Empty;
    }
}
