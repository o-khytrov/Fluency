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
            var matched = false;
            while (!matched)
            {
                isMatch = element.Match(tokenCollection, extracted);
                matched = isMatch;
            }

            if (!matched)
            {
                break;
            }
        }

        return new PatternMatchingResult(isMatch, extracted);
    }
}