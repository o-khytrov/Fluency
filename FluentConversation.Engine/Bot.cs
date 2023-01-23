using FluentConversation.Engine.Rules;
using Mosaik.Core;

namespace FluentConversation.Engine;

public class Bot<T> where T : new()
{
    public Language Language { get; set; }

    public T Context { get; set; }

    public List<BotRule<T>> BotRules { get; set; } = new();

    public string ChatCompleteMessage { get; set; } = "Chat is completed";


    protected RuleBuilder<T> R(string? name = null)
    {
        var rule = new BotRule<T>
        {
            Name = name
        };
        BotRules.Add(rule);
        return new RuleBuilder<T>(rule);
    }


    protected T? When(Func<T, bool> predicate, Action action)
    {
        return default(T);
    }


    public string OneOf(params string[] words)
    {
        var index = new Random().Next(0, words.Length);
        return words[index];
    }
}