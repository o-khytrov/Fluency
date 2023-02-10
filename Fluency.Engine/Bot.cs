using Fluency.Engine.Models;
using Fluency.Engine.PatternSystem;
using Fluency.Engine.Rules;
using Fluency.Engine.Tokenization;
using Mosaik.Core;

namespace Fluency.Engine;

public abstract class Bot
{
    public abstract Type Type { get; }
}

public abstract class Bot<T> : Bot where T : ChatContext, new()
{
    public override Type Type => typeof(T);

    public abstract Language Language { get; }

    public abstract string Name { get; }

    private Dictionary<string, Topic<T>> Topics { get; set; } = new();

    internal virtual string ChatCompleteMessage { get; set; } = "Chat is completed";


    /// <summary>
    /// Create a gambit rule
    /// </summary>
    /// <param name="name">Rule name</param>
    /// <param name="keep"></param>
    /// <param name="repeat">Allow same output multiple times</param>
    /// <returns></returns>
    protected IRuleBuilderOutputStage<T> G(string? name = null, bool keep = false, bool repeat = false)
    {
        return CreateNewBootRule(name, keep, repeat);
    }

    /// <summary>
    /// Create a rule, a rule which reacts to user input 
    /// </summary>
    /// <param name="name">Rule name</param>
    /// <param name="keep"></param>
    /// <param name="repeat">Allow same output multiple times</param>
    /// <returns></returns>
    protected IRuleBuilderInitialStage<T> R(string? name = null, bool keep = false, bool repeat = false)
    {
        return CreateNewBootRule(name, keep, repeat);
    }

    private RuleBuilder<T> CreateNewBootRule(string? name, bool keep, bool repeat)
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


    protected string OneOf(params string[] words)
    {
        var index = new Random().Next(0, words.Length);
        return words[index];
    }

    private bool HasTopic(string topicName)
    {
        return Topics.ContainsKey(topicName);
    }

    public Topic<T> GetTopic(string topicName)
    {
        return Topics[topicName];
    }

    public virtual BotMessage? Control(PatternEngine scanner, Conversation<T> conversation, BotInput botInput)
    {
        if (conversation.PendingRejoinders.Any())
        {
            var response = Respond(scanner, conversation, conversation.PendingRejoinders, botInput);
            if (response is not null)
            {
                return response;
            }
        }

        var rules = new List<BotRule<T>>();
        var topicRules = Topics[conversation.CurrentTopic].BotRules;
        rules.AddRange(topicRules);

        return Respond(scanner, conversation, rules, botInput);
    }

    private BotMessage? Respond(PatternEngine scanner, Conversation<T> conversation, List<BotRule<T>> rules, BotInput botInput)
    {
        foreach (var rule in rules)
        {
            if (conversation.RuleShown.Contains(rule) && !rule.Keep)
            {
                continue;
            }

            foreach (var preAction in rule.PreActions)
            {
                preAction.Invoke(botInput, conversation.Context);
            }

            if (rule.IsPreConditionTrue(conversation.Context, botInput))
            {
                var isMatch = true;
                if (rule.Pattern is not null)
                {
                    var matchingResult = scanner.Match(rule.Pattern, botInput);
                    isMatch = matchingResult.Match;
                    if (matchingResult.Match)
                    {
                        foreach (var postAction in rule.PostActions)
                        {
                            postAction(conversation.Context, matchingResult);
                        }
                    }
                }

                if (isMatch)
                {
                    conversation.PendingRejoinders.Clear();
                    if (rule.Rejoinders.Any())
                    {
                        conversation.PendingRejoinders.AddRange(rule.Rejoinders);
                    }

                    if (!string.IsNullOrEmpty(rule.NexTopic) && HasTopic(rule.NexTopic))
                    {
                        conversation.CurrentTopic = rule.NexTopic;
                    }

                    conversation.RuleShown.Add(rule);

                    var botMessage = new BotMessage
                    {
                        RuleName = rule.Name,
                        Text = rule.RenderOutput(conversation.Context)
                    };
                    return botMessage;
                }
            }
        }

        return null;
    }
}