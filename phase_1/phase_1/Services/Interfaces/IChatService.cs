using phase_1.DTOs;

namespace phase_1.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatResponse> SendMessageAsync(ChatRequest request);
    }
}
