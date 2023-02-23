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
    protected IGambitBuilder<T> G(string? name = null, bool keep = false, bool repeat = false)
    {
        return CreateNewBootRule(name, RuleType.Gambit, keep, repeat);
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
        return CreateNewBootRule(name, RuleType.Responder, keep, repeat);
    }

    private RuleBuilder<T> CreateNewBootRule(string? name, RuleType type, bool keep, bool repeat)
    {
        var rule = new BotRule<T>
        {
            Name = name,
            Keep = keep,
            Repeat = repeat,
            Type = type
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
            rule.Keep = rule.Keep || keep;
            rule.Repeat = rule.Repeat || repeat;
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

    public virtual BotMessage? Control(PatternEngine scanner, Conversation<T> conversation)
    {
        var responseCount = conversation.CurrentVolley.ResponseCount;
        if (conversation.PendingRejoinders.Any())
        {
            Respond(scanner, conversation, conversation.PendingRejoinders);
        }

        if (responseCount == conversation.CurrentVolley.ResponseCount)
        {
            //Current topic tries to respond to this input
            Respond(scanner, conversation, Topics[conversation.CurrentTopic]);
        }

        if (responseCount == conversation.CurrentVolley.ResponseCount)
        {
            Gambit(scanner, conversation, Topics[conversation.CurrentTopic].BotRules.ToList());
        }

        // see if some other topic has keywords matching his input (given we have no response yet)
        if (responseCount == conversation.CurrentVolley.ResponseCount)
        {
            var keywordsTopics = GetKeywordTopics(conversation.CurrentInput);
            foreach (var keywordTopic in keywordsTopics)
            {
                Respond(scanner, conversation, keywordTopic);

                if (responseCount != conversation.CurrentVolley.ResponseCount)
                {
                    return FinishVolley(conversation);
                }
            }
        }

        if (responseCount == conversation.CurrentVolley.ResponseCount)
        {
            var gambitTopics = GambitTopics();
            foreach (var gambitTopic in gambitTopics)
            {
                Gambit(scanner, conversation, gambitTopic.BotRules.ToList());

                if (responseCount != conversation.CurrentVolley.ResponseCount)
                {
                    return FinishVolley(conversation);
                }
            }
        }

        if (responseCount == conversation.CurrentVolley.ResponseCount)
        {
            return new BotMessage
                { Text = "I don't know what to say" };
        }

        return FinishVolley(conversation);
    }

    private void Gambit(PatternEngine scanner, Conversation<T> conversation, List<BotRule<T>> rules)
    {
        var gambit = rules.FirstOrDefault(x => x.Type == RuleType.Gambit
                                               && (!conversation.RuleShown.Contains(x) || x.Keep)
                                               && x.IsPreConditionTrue(conversation));
        if (gambit is not null)
        {
            PushRejoinders(conversation, gambit);

            conversation.RuleShown.Add(gambit);
            conversation.CurrentVolley.AddOutput(gambit.RenderOutput(conversation.Context));
        }
    }

    private void Respond(PatternEngine scanner, Conversation<T> conversation, Topic<T> topic)
    {
        Respond(scanner, conversation, topic.BotRules.ToList());
    }

    private void Respond(PatternEngine scanner, Conversation<T> conversation, List<BotRule<T>> rules)
    {
        foreach (var rule in rules.Where(x => x.Type == RuleType.Responder))
        {
            if (conversation.RuleShown.Contains(rule) && !rule.Keep)
            {
                continue;
            }

            foreach (var preAction in rule.PreActions)
            {
                preAction.Invoke(conversation.CurrentInput, conversation.Context);
            }

            if (rule.IsPreConditionTrue(conversation))
            {
                var isMatch = true;
                if (rule.Pattern is not null)
                {
                    var matchingResult = scanner.Match(rule.Pattern, conversation);
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
                    PushRejoinders(conversation, rule);

                    if (!string.IsNullOrEmpty(rule.NexTopic) && HasTopic(rule.NexTopic))
                    {
                        conversation.CurrentTopic = rule.NexTopic;
                    }

                    var output = rule.RenderOutput(conversation.Context);
                    if (string.IsNullOrWhiteSpace(output)) continue;
                    conversation.RuleShown.Add(rule);
                    conversation.CurrentVolley.AddOutput(output);
                    return;
                }
            }
        }
    }

    private static void PushRejoinders(Conversation<T> conversation, BotRule<T> rule)
    {
        conversation.PendingRejoinders.Clear();
        if (rule.Rejoinders.Any())
        {
            conversation.PendingRejoinders.AddRange(rule.Rejoinders);
        }
    }

    private IEnumerable<Topic<T>> GetKeywordTopics(BotInput input)
    {
        return Topics.Values;
    }

    private BotMessage FinishVolley(Conversation<T> conversation)
    {
        var botMessage = new BotMessage
        {
            Text = conversation.CurrentVolley.GetResponse()
        };
        return botMessage;
    }

    private IEnumerable<Topic<T>> GambitTopics()
    {
        return Topics.Values.Where(x => x.BotRules.Any(x => x.Type == RuleType.Gambit));
    }
}