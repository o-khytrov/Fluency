using Catalyst;
using FluentConversation.Engine.PatternSystem.Elements;

namespace FluentConversation.Engine.PatternSystem;

public class PatternBuilder
{
    private readonly Pattern _pattern;
    private readonly Tokenizer _tokenizer;

    public PatternBuilder()
    {
        _pattern = new Pattern();
        _tokenizer = new Tokenizer();
    }


    /// <summary>
    /// Disjunction: match one of provided patterns
    /// </summary>
    /// <param name="patternBuilderAction"></param>
    /// <returns></returns>
    public PatternBuilder Or(params Action<PatternBuilder>[] patternBuilderAction)
    {
        var patterns = new List<Pattern>();
        foreach (var builderAction in patternBuilderAction)
        {
            var builder = new PatternBuilder();
            builderAction.Invoke(builder);
            patterns.Add(builder.Build());
        }

        _pattern.Elements.Add(new Disjunction(patterns));
        return this;
    }

    /// <summary>
    /// Matches word in canonical form
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public PatternBuilder Lemma(string word)
    {
        _pattern.Elements.Add(new SingleWordPatternElement
        {
            PatternElementType = PatternElementType.Word, Value = word.Trim(), Lemma = true
        });
        return this;
    }

    /// <summary>
    /// Match single word
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public PatternBuilder Word(string word)
    {
        _pattern.Elements.Add(new SingleWordPatternElement
        {
            PatternElementType = PatternElementType.Word, Value = word.Trim(),
        });
        return this;
    }

    /// <summary>
    /// Match one one provided words 
    /// </summary>
    /// <param name="words"></param>
    /// <returns></returns>
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