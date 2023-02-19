namespace Fluency.Api.Models;

public class PerformStepResponse
{
    public string? Text { get; set; }

    public Dictionary<string, object> Variables { get; set; } = new();
}