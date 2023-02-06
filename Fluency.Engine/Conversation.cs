using FluentConversation.Engine.Models;
using FluentConversation.Engine.Rules;

namespace FluentConversation.Engine;

public class Conversation<T> where T : new()
{
    public List<BotRule<T>> RuleShown { get; set; } = new();

    public T Context { get; set; } = new();

    public List<Message> Messages { get; set; } = new();

    public string UserId { get; set; } = string.Empty;
}