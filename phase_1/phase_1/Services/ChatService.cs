using phase_1.DTOs;
using phase_1.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace phase_1.Services
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly ICompanySettingService _companySettingService;
        private readonly IProductService _productService;

        public ChatService(HttpClient httpClient, IConfiguration configuration, IWebHostEnvironment env, ICompanySettingService companySettingService, IProductService productService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _env = env;
            _companySettingService = companySettingService;
            _productService = productService;
        }

        public async Task<ChatResponse> SendMessageAsync(ChatRequest request)
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            var model = _configuration["Gemini:Model"] ?? "gemini-2.5-flash";
            
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var settings = await _companySettingService.GetSettingsAsync();

            var chatScriptPath = Path.Combine(_env.ContentRootPath, "..", "..", "chat.md");
            string systemInstruction = "Bạn là trợ lý ảo hỗ trợ khách hàng của cửa hàng văn phòng phẩm.";
            if (File.Exists(chatScriptPath))
            {
                var content = await File.ReadAllTextAsync(chatScriptPath);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    systemInstruction = content
                        .Replace("{{CompanyName}}", settings.CompanyName ?? "")
                        .Replace("{{Address}}", settings.Address ?? "")
                        .Replace("{{Hotline}}", settings.Hotline ?? "")
                        .Replace("{{Email}}", settings.Email ?? "")
                        .Replace("{{WorkingHours}}", settings.WorkingHours ?? "");
                }
            }

            var systemInstructionObj = new
            {
                parts = new[] { new { text = systemInstruction } }
            };

            var toolsObj = new[]
            {
                new
                {
                    function_declarations = new[]
                    {
                        new
                        {
                            name = "search_products",
                            description = "Tìm kiếm thông tin sản phẩm (tên, giá cả, số lượng tồn kho) bằng từ khóa để báo cho khách hàng.",
                            parameters = new
                            {
                                type = "OBJECT",
                                properties = new
                                {
                                    keyword = new
                                    {
                                        type = "STRING",
                                        description = "Từ khóa tìm kiếm (ví dụ: bút bi, sổ tay, thiên long...)"
                                    }
                                },
                                required = new[] { "keyword" }
                            }
                        }
                    }
                }
            };

            var requestBody = new
            {
                system_instruction = systemInstructionObj,
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[] { new { text = request.Message } }
                    }
                },
                tools = toolsObj
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                var errorString = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Error: {errorString}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseString);
            var root = doc.RootElement;
            
            if (!root.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
            {
                return new ChatResponse { Response = "Xin lỗi, tôi không thể trả lời câu hỏi này do vấn đề an toàn nội dung." };
            }

            var contentProp = candidates[0].GetProperty("content");
            if (!contentProp.TryGetProperty("parts", out var parts) || parts.GetArrayLength() == 0)
            {
                return new ChatResponse { Response = "Xin lỗi, tôi không thể xử lý yêu cầu của bạn lúc này." };
            }

            if (parts[0].TryGetProperty("functionCall", out var functionCall))
            {
                var functionName = functionCall.GetProperty("name").GetString();
                if (functionName == "search_products")
                {
                    string keyword = "";
                    if (functionCall.TryGetProperty("args", out var argsProp) && argsProp.TryGetProperty("keyword", out var keywordProp))
                    {
                        keyword = keywordProp.GetString() ?? "";
                    }
                    
                    var products = await _productService.SearchProductsAsync(keyword ?? "");
                    var productList = products.Select(p => new {
                        p.Name,
                        Price = p.Price.ToString("C0", new System.Globalization.CultureInfo("vi-VN")),
                        p.StockQuantity,
                        Category = p.Category?.Name
                    }).Take(5).ToList();
                    var secondRequestBody = new
                    {
                        system_instruction = systemInstructionObj,
                        contents = new object[]
                        {
                            new
                            {
                                role = "user",
                                parts = new[] { new { text = request.Message } }
                            },
                            new
                            {
                                role = "model",
                                parts = new[] { new { functionCall = new { name = "search_products", args = new { keyword = keyword } } } }
                            },
                            new
                            {
                                role = "function",
                                parts = new[]
                                {
                                    new
                                    {
                                        functionResponse = new
                                        {
                                            name = "search_products",
                                            response = new { name = "search_products", content = productList }
                                        }
                                    }
                                }
                            }
                        },
                        tools = toolsObj
                    };

                    var secondJsonContent = new StringContent(JsonSerializer.Serialize(secondRequestBody), Encoding.UTF8, "application/json");
                    var secondResponse = await _httpClient.PostAsync(url, secondJsonContent);
                    
                    if (!secondResponse.IsSuccessStatusCode)
                    {
                        var errStr = await secondResponse.Content.ReadAsStringAsync();
                        throw new Exception($"Gemini API Error (Second Call): {errStr}");
                    }

                    var secondRespStr = await secondResponse.Content.ReadAsStringAsync();
                    using var secondDoc = JsonDocument.Parse(secondRespStr);
                    var secondRoot = secondDoc.RootElement;
                    
                    if (!secondRoot.TryGetProperty("candidates", out var secCandidates) || secCandidates.GetArrayLength() == 0)
                    {
                        return new ChatResponse { Response = "Xin lỗi, tôi không thể trả lời câu hỏi này." };
                    }
                    
                    var secContent = secCandidates[0].GetProperty("content");
                    if (!secContent.TryGetProperty("parts", out var secParts) || secParts.GetArrayLength() == 0)
                    {
                        return new ChatResponse { Response = "Xin lỗi, tôi không thể xử lý yêu cầu của bạn lúc này." };
                    }

                    var textResponse = secParts[0].GetProperty("text").GetString();
                    return new ChatResponse { Response = textResponse ?? string.Empty };
                }
            }

            if (parts[0].TryGetProperty("text", out var textProp))
            {
                return new ChatResponse { Response = textProp.GetString() ?? string.Empty };
            }
            
            return new ChatResponse { Response = "Xin lỗi, tôi không hiểu yêu cầu của bạn." };
        }
    }
}
