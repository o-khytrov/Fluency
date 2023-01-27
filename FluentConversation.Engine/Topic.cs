using FluentConversation.Engine.Rules;

namespace FluentConversation.Engine;

public class Topic<T>
{
    public List<BotRule<T>> BotRules { get; set; } = new();

    public string Name { get; set; } = string.Empty;
}