using FluentConversation.Engine.PatternSystem.Elements;
using FluentConversation.Engine.Tokenization;

namespace FluentConversation.Engine.PatternSystem;

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

    public DisjunctionBuilder Disjunction(Action<DisjunctionBuilder> builderAction)
    {
        var disjunctionBuilder = new DisjunctionBuilder();
        builderAction(disjunctionBuilder);
        _pattern.Elements.Add(disjunctionBuilder.Build());
        return disjunctionBuilder;
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