using Fluency.Engine.Rules;

namespace Fluency.Engine;

public class Topic<T>
    where T : ChatContext, new()
{
    public Topic(string name)
    {
        Name = name;
    }

    internal TrackingCollection<BotRule<T>> BotRules { get; set; } = new();

    public string Name { get; set; }
}