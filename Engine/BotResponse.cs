namespace Engine;

public class BotResponse
{
    public string Text { get; set; } = string.Empty;

    public Dictionary<string, object> Variables { get; set; } = new();
}