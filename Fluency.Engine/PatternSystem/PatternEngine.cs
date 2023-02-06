using FluentConversation.Engine.PatternSystem.Elements;
using FluentConversation.Engine.Tokenization;

namespace FluentConversation.Engine.PatternSystem;

public class PatternEngine
{
    private readonly Tokenizer _tokenizer;

    public PatternEngine(Tokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    public PatternMatchingResult Match(Pattern pattern, BotInput input)
    {
        var extracted = new List<string>();
        var isMatch = true;
        input.Reset();
        foreach (var element in pattern.Elements)
        {
            var currentRuleMatch = false;
            while (!currentRuleMatch && input.CanMoveNext())
            {
                currentRuleMatch = element.Match(input, extracted, _tokenizer);
            }

            if (!currentRuleMatch)
            {
                isMatch = false;
                break;
            }
        }

        return new PatternMatchingResult(isMatch, extracted);
    }
}