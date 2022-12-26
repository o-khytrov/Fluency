using Engine.Tokenization;

namespace Engine.PatternSystem.Elements;

public class ConceptPatternElement : PatternElement
{
    public List<string>? Values { get; set; }

    public override bool Match(BotInput input, List<string> extracted)
    {
        throw new NotImplementedException();
    }
}