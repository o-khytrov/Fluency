using Catalyst;
using Fluency.Engine.Tokenization;

namespace Fluency.Engine.PatternSystem.Elements;

public class PartOfSpeechPatternElement : PatternElement
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

        return token.POS == _partOfSpeech;
    }
}