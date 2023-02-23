using Fluency.Engine;

namespace SampleBots.Travelling;

public class TravelContext : ChatContext
{
    /// <summary>
    /// Current location of the user
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Place where the user wants to go
    /// </summary>
    public string? Target { get; set; }
}