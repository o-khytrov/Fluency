using Fluency.Engine.Rules;
using Mosaik.Core;

namespace Fluency.Engine;

public abstract class Bot
{
    public abstract Type Type { get; }
}

public abstract class Bot<T> : Bot
{
    public override Type Type => typeof(T);

    public abstract Language Language { get; }

    public abstract string Name { get; }

    protected Dictionary<string, Topic<T>> Topics { get; set; } = new();

    public string ChatCompleteMessage { get; set; } = "Chat is completed";


    /// <summary>
    /// Create a rule, a rule which reacts to user input 
    /// </summary>
    /// <param name="name">Rule name</param>
    /// <param name="keep"></param>
    /// <param name="repeat">Allow same output multiple times</param>
    /// <returns></returns>
    protected IRuleBuilderInitialStage<T> R(string? name = null, bool keep = false, bool repeat = false)
    {
        var rule = new BotRule<T>
        {
            Name = name,
            Keep = keep,
            Repeat = repeat
        };
        if (!Topics.ContainsKey(Constants.DefaultTopic))
        {
            Topics.Add(Constants.DefaultTopic, new Topic<T>(Constants.DefaultTopic));
        }

        var topic = Topics[Constants.DefaultTopic];
        topic.BotRules.Add(rule);
        return new RuleBuilder<T>(rule, topic);
    }

    protected void Topic(string name, Action action, bool keep = false, bool repeat = false)
    {
        if (!Topics.ContainsKey(name))
        {
            Topics.Add(name, new Topic<T>(name));
        }

        var tempContainer = new List<BotRule<T>>();
        using (Topics[Constants.DefaultTopic].BotRules.Capture(tempContainer.Add))
        {
            action();
        }

        foreach (var rule in tempContainer)
        {
            rule.Keep = keep;
            rule.Repeat = repeat;
        }

        Topics[name].BotRules.AddRange(tempContainer);
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

    public bool HasTopic(string topicName)
    {
        return Topics.ContainsKey(topicName);
    }

    public Topic<T> GetTopic(string topicName)
    {
        return Topics[topicName];
    }
}