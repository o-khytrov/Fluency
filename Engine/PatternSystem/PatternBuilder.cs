using Engine.PatternSystem.Elements;
using Engine.Tokenization;

namespace Engine.PatternSystem;

public class PatternBuilder
{
    private readonly Pattern _pattern;

    public PatternBuilder()
    {
        _pattern = new Pattern();
    }

    public PatternBuilder Word(string word)
    {
        _pattern.Elements.Add(new SingleWordPatternElement
        {
            PatternElementType = PatternElementType.Word, Value = word.Trim()
        });
        return this;
    }

    public PatternBuilder Word(params string[] words)
    {
        _pattern.Elements.Add(new MultipleWordsPatternElement
        {
            PatternElementType = PatternElementType.Word,
            Values = words.Select(x => x.Trim()).ToList()
        });
        return this;
    }

    public PatternBuilder Phrase(string phrase)
    {
        var tokens = new BotInput(phrase);
        _pattern.Elements.Add(new PhrasePatternElement(tokens) { PatternElementType = PatternElementType.Word });
        return this;
    }

    public PatternBuilder Wildcard(Action<string>? resultAction = null)
    {
        _pattern.Elements.Add(new WildcardPatternElement(resultAction)
        {
            PatternElementType = PatternElementType.Wildcard
        });
        return this;
    }

    public PatternBuilder Concept<T>() where T : Concept
    {
        _pattern.Elements.Add(new ConceptPatternElement
        {
            PatternElementType = PatternElementType.Word
        });
        return this;
    }

    public Pattern Build()
    {
        return _pattern;
    }
}