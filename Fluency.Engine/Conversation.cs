using Fluency.Engine.Models;
using Fluency.Engine.Rules;

namespace Fluency.Engine;

public class Conversation<T> where T : new()
{
    public T Context { get; set; } = new();

    public List<BotRule<T>> RuleShown { get; set; } = new();

    public List<BotRule<T>> PendingRejoinders { get; set; } = new();

    public List<Message> Messages { get; set; } = new();

    public string UserId { get; set; } = string.Empty;

    public string CurrentTopic { get; set; }

    public int Input { get; set; }

    public int Response { get; set; }
}