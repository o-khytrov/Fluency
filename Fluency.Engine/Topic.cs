using Fluency.Engine.Rules;

namespace Fluency.Engine;

public class Topic<T>
{
    internal TrackingCollection<BotRule<T>> BotRules { get; set; } = new();

    public string Name { get; set; } = string.Empty;
}