using Fluency.Engine.Rules;
using Mosaik.Core;

namespace Fluency.Engine;

public abstract class Bot<T>
{
    public abstract Language Language { get; }

    public abstract string Name { get; }

    public Topic<T> DefaultTopic { get; set; } = new();

    public List<Topic<T>> Topics { get; set; } = new() { new Topic<T>() { Name = "Default" } };

    public string ChatCompleteMessage { get; set; } = "Chat is completed";


    protected RuleBuilder<T> R(string? name = null)
    {
        var rule = new BotRule<T>
        {
            Name = name
        };
        DefaultTopic.BotRules.Add(rule);
        return new RuleBuilder<T>(rule, DefaultTopic);
    }

    public void Topic(string name, Action action)
    {
        var rule = new BotRule<T>
        {
            Name = name
        };
        DefaultTopic.BotRules.Add(rule);
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