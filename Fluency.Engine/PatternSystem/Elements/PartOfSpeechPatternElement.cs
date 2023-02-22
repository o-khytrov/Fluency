using Catalyst;
using Fluency.Engine.Tokenization;

namespace Fluency.Engine.PatternSystem.Elements;

public class PartOfSpeechPatternElement<T> : PatternElement<T> where T : ChatContext, new()
{
    private readonly PartOfSpeech _partOfSpeech;

    public PartOfSpeechPatternElement(PartOfSpeech partOfSpeech)
    {
        _partOfSpeech = partOfSpeech;
    }

    public override bool Match(BotInput input, List<string> extracted, Tokenizer tokenizer)
    {
        if (!input.MoveNext())
        {
            return false;
        }


        var token = input.Current;

        var isMatch = token.POS == _partOfSpeech;
        if (isMatch)
        {
            extracted.Add(token.Value);
        }

        return isMatch;
    }
}