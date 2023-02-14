using Catalyst;
using Fluency.Engine.Fact;
using Fluency.Engine.PatternSystem.Elements;

namespace Fluency.Engine.PatternSystem;

public class PatternBuilder
{
    private readonly Pattern _pattern;

    public PatternBuilder()
    {
        _pattern = new Pattern();
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
            Value = word.Trim(), Lemma = true
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
            Value = word.Trim(),
        });
        return this;
    }

    /// <summary>
    /// Match single word
    /// </summary>
    /// <param name="word"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public PatternBuilder Word(string word, Func<IToken, bool> predicate)
    {
        _pattern.Elements.Add(new SingleWordPatternElement(predicate)
        {
            Value = word.Trim(),
        });
        return this;
    }

    public PatternBuilder Pos(PartOfSpeech partOfSpeech)
    {
        _pattern.Elements.Add(new PartOfSpeechPatternElement(partOfSpeech));
        return this;
    }

    public PatternBuilder Pronoun()
    {
        _pattern.Elements.Add(new PartOfSpeechPatternElement(PartOfSpeech.PRON));
        return this;
    }

    public PatternBuilder Noun()
    {
        _pattern.Elements.Add(new PartOfSpeechPatternElement(PartOfSpeech.NOUN));
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
            Values = words.Select(x => x.Trim()).ToList()
        });
        return this;
    }

    public PatternBuilder Phrase(string phrase, bool ignoreOrder = false)
    {
        _pattern.Elements.Add(new PhrasePatternElement(phrase, arbitraryOrder: ignoreOrder));
        return this;
    }

    public PatternBuilder Wildcard(int number = 1, Action<IToken>? resultAction = null)
    {
        _pattern.Elements.Add(new WildcardPatternElement(number, resultAction));
        return this;
    }

    public PatternBuilder Concept<T>() where T : Concept
    {
        _pattern.Elements.Add(new ConceptPatternElement());
        return this;
    }

    public Pattern Build()
    {
        return _pattern;
    }
}