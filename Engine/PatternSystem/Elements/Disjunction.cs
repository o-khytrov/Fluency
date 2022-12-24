using Engine.Tokenization;

namespace Engine.PatternSystem.Elements;

public class Disjunction : PatternElement
{
    private readonly Pattern _pattern;

    public Disjunction()
    {
        _pattern =  new Pattern();
    }

    public override bool Match(BotInput input, List<string> extracted)
    {
        foreach (var element in _pattern.Elements)
        {
            var match = element.Match(input, extracted);
            if (match)
            {
                return true;
            }
        }

        return false;
    }

    public void AddElement(PatternElement element)
    {
        this._pattern.Elements.Add(element);
    }
}