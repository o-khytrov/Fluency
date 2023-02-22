using Fluency.Engine.PatternSystem;
using Fluency.Engine.PatternSystem.Elements;
using Fluency.Engine.Tokenization;

namespace Fluency.Engine.Rules;

public enum RuleType
{
    Gambit,
    Responder
}

public class BotRule<T>
    where T : ChatContext, new()
{
    public List<Func<T, BotInput, bool>> Conditions { get; set; } = new();

    public List<Func<Conversation<T>, BotInput, bool>> ConversationConditions { get; set; } = new();

    public RuleType Type { get; set; }

    public string NexTopic { get; set; }

    public readonly List<Action<T, PatternMatchingResult>> PostActions = new();
    public List<Action<BotInput, T>> PreActions = new();

    public Pattern<T>? Pattern { get; set; }


    private Func<T, string>? _renderOutput;

    public string? RenderOutput(T context)
    {
        if (_renderOutput is not null)
        {
            return _renderOutput(context);
        }

        return null;
    }

    public List<RuleTest> Tests { get; set; } = new();

    public readonly List<BotRule<T>> Rejoinders = new();

    public bool Keep { get; set; }

    public bool Repeat { get; set; }

    public string? Name { get; set; }

    public void SetOutputRenderer(Func<T, string> renderer)
    {
        _renderOutput = renderer;
    }

    public bool IsPreConditionTrue(Conversation<T> conversation, BotInput input)
    {
        var isMatch = true;
        foreach (var condition in Conditions)
        {
            if (!condition(conversation.Context, input))
            {
                isMatch = false;
                break;
            }
        }

        foreach (var condition in ConversationConditions)
        {
            if (!condition(conversation, input))
            {
                isMatch = false;
                break;
            }
        }

        return isMatch;
    }
}

public class RuleTest
{
    public string Input { get; set; } = string.Empty;

    public string ExpectedResponse { get; set; } = string.Empty;
}