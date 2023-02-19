namespace Fluency.Api.Models;

public class CreateChatRequest
{
    public string DialogueName { get; set; } = string.Empty;

    public Dictionary<string, object> Variables { get; set; } = new();

    public string Text { get; set; } = string.Empty;
}