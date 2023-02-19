namespace Fluency.Api.Models;

public class PerformStepRequest
{
    public string ChatId { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public Dictionary<string, object> Variables { get; set; } = new();
}