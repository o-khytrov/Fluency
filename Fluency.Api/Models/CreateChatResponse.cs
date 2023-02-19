namespace Fluency.Api.Models;

public class CreateChatResponse
{
    public string ChatId { get; set; } = string.Empty;

    public string? Text { get; set; }

    public Dictionary<string, object> Variables { get; set; } = new();
}