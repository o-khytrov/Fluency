using Engine.Tokenization;

namespace Engine.PatternSystem.Elements;

public class PhrasePatternElement : PatternElement
{
    public string? Value { get; set; }

    public override bool Match(TokenCollection input, List<string> extracted)
    {
        throw new NotImplementedException();
    }
}