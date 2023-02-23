using Fluency.Engine.Tokenization;

namespace Fluency.Engine.PatternSystem.Elements;

public class Disjunction<T> : PatternElement<T> where T : ChatContext, new()
{
    private readonly List<Pattern<T>> _patterns;

    public Disjunction(List<Pattern<T>> patterns)
    {
        _patterns = patterns;
    }

    public override bool Match(Conversation<T> conversation, List<string> extracted, Tokenizer tokenizer)
    {
        foreach (var childPattern in _patterns)
        {
            conversation.CurrentInput.Reset();
            var match = MatchChildPattern(conversation, childPattern, extracted, tokenizer);
            if (match)
            {
                return true;
            }
        }

        return false;
    }

    private bool MatchChildPattern(Conversation<T> conversation, Pattern<T> childPattern, List<string> extracted,
        Tokenizer tokenizer)
    {
        var input = conversation.CurrentInput;
        var matched = true;
        foreach (var element in childPattern.Elements)
        {
            var currentRuleMatch = false;
            input.Reset();
            while (!currentRuleMatch && input.CanMoveNext())
            {
                currentRuleMatch = element.Match(conversation, extracted, tokenizer);
            }

            if (!currentRuleMatch)
            {
                matched = false;
                break;
            }
        }

        return matched;
    }
}