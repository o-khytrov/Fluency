using System.Linq.Expressions;
using Catalyst;
using Fluency.Engine.PatternSystem.Elements;

namespace Fluency.Engine.PatternSystem;

public class PatternBuilder<T>
    where T : ChatContext, new()
{
    private readonly Pattern<T> _pattern;

    public PatternBuilder()
    {
        _pattern = new Pattern<T>();
    }

    /// <summary>
    /// Disjunction: match one of provided patterns
    /// </summary>
    /// <param name="patternBuilderAction"></param>
    /// <returns></returns>
    public PatternBuilder<T> Or(params Action<PatternBuilder<T>>[] patternBuilderAction)
    {
        var patterns = new List<Pattern<T>>();
        foreach (var builderAction in patternBuilderAction)
        {
            var builder = new PatternBuilder<T>();
            builderAction.Invoke(builder);
            patterns.Add(builder.Build());
        }

        _pattern.Elements.Add(new Disjunction<T>(patterns));
        return this;
    }

    /// <summary>
    /// Matches word in canonical form
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public PatternBuilder<T> Lemma(string word)
    {
        _pattern.Elements.Add(new SingleWordPatternElement<T>
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
    public PatternBuilder<T> Word(string word)
    {
        _pattern.Elements.Add(new SingleWordPatternElement<T>
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
    public PatternBuilder<T> Word(string word, Func<IToken, bool> predicate)
    {
        _pattern.Elements.Add(new SingleWordPatternElement<T>(predicate)
        {
            Value = word.Trim(),
        });
        return this;
    }

    public PatternBuilder<T> Pos(PartOfSpeech partOfSpeech)
    {
        _pattern.Elements.Add(new PartOfSpeechPatternElement<T>(partOfSpeech));
        return this;
    }

    public PatternBuilder<T> Pronoun()
    {
        _pattern.Elements.Add(new PartOfSpeechPatternElement<T>(PartOfSpeech.PRON));
        return this;
    }
    
    public PatternBuilder<T> Verb()
    {
        _pattern.Elements.Add(new PartOfSpeechPatternElement<T>(PartOfSpeech.VERB));
        return this;
    }

    public PatternBuilder<T> Noun()
    {
        _pattern.Elements.Add(new PartOfSpeechPatternElement<T>(PartOfSpeech.NOUN));
        return this;
    }

    /// <summary>
    /// Match one one provided words 
    /// </summary>
    /// <param name="words"></param>
    /// <returns></returns>
    public PatternBuilder<T> Word(params string[] words)
    {
        _pattern.Elements.Add(new MultipleWordsPatternElement<T>
        {
            Values = words.Select(x => x.Trim()).ToList()
        });
        return this;
    }

    public PatternBuilder<T> Phrase(string phrase, bool ignoreOrder = false)
    {
        _pattern.Elements.Add(new PhrasePatternElement<T>(phrase, arbitraryOrder: ignoreOrder));
        return this;
    }

    public PatternBuilder<T> Wildcard()
    {
        _pattern.Elements.Add(new WildcardPatternElement<T>(1, null));
        return this;
    }

    public PatternBuilder<T> Wildcard(Action<T, string> action = null)
    {
        _pattern.Elements.Add(new WildcardPatternElement<T>(1, assign: action));
        return this;
    }

    public PatternBuilder<T> Wildcard(int number = 1, Action<T, string> action = null)
    {
        _pattern.Elements.Add(new WildcardPatternElement<T>(number, assign: action));
        return this;
    }

    /*
    public PatternBuilder<T> Wildcard<TProperty>(int number = 1,
        Expression<Func<T, TProperty>>? expression = null)
    {
        if (expression is not null)
        {
            var propertyInfo = expression.GetMember();
        }

        _pattern.Elements.Add(new WildcardPatternElement<T>(number, null));
        return this;
    }
    */
    /*
    public PatternBuilder<T> Wildcard(int number = 1, Action<IToken>? resultAction = null)
    {
        _pattern.Elements.Add(new WildcardPatternElement<T>(number, resultAction));
        return this;
    }
*/
    public PatternBuilder<T> Concept()
    {
        _pattern.Elements.Add(new ConceptPatternElement<T>());
        return this;
    }

    public Pattern<T> Build()
    {
        return _pattern;
    }
}