using Catalyst;
using FluentConversation.Engine.PatternSystem.Elements;

namespace FluentConversation.Engine.PatternSystem;

public class PatternBuilder
{
    private readonly Pattern _pattern;
    private Tokenizer _tokenizer;

    public PatternBuilder()
    {
        _pattern = new Pattern();
        _tokenizer = new Tokenizer();
    }


    public DisjunctionBuilder Disjunction(Action<DisjunctionBuilder> builderAction)
    {
        var disjunctionBuilder = new DisjunctionBuilder();
        builderAction(disjunctionBuilder);
        _pattern.Elements.Add(disjunctionBuilder.Build());
        return disjunctionBuilder;
    }

    public PatternBuilder Lemma(string word)
    {
        _pattern.Elements.Add(new SingleWordPatternElement
        {
            PatternElementType = PatternElementType.Word, Value = word.Trim(), Lemma = true
        });
        return this;
    }

    public PatternBuilder Word(string word)
    {
        _pattern.Elements.Add(new SingleWordPatternElement
        {
            PatternElementType = PatternElementType.Word, Value = word.Trim(),
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
        var tokens = _tokenizer.Tokenize(phrase).ToTokenList();
        _pattern.Elements.Add(new PhrasePatternElement(tokens) { PatternElementType = PatternElementType.Word });
        return this;
    }

    public PatternBuilder Wildcard(Action<IToken>? resultAction = null)
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