using Engine.PatternSystem.Elements;
using Engine.Tokenization;

namespace Engine.PatternSystem;

public class PatternEngine
{
    public PatternMatchingResult Match(Pattern pattern, string input)
    {
        var tokens = Tokenizer.TokenizeStrings(input);
        var tokenCollection = new TokenCollection(tokens);

        var extracted = new List<string>();
        var isMatch = true;
        foreach (var element in pattern.Elements)
        {
            var currentRuleMatch = false;
            while (!currentRuleMatch && tokenCollection.CanMoveNext())
            {
                currentRuleMatch = element.Match(tokenCollection, extracted);
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