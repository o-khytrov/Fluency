using Catalyst;
using Fluency.Engine.Tokenization;

namespace Fluency.Engine.PatternSystem.Elements;

public class PhrasePatternElement : PatternElement
{
    private List<IToken>? _phraseTokens;

    private readonly StringComparison _stringComparison;
    private readonly string _phrase;
    private readonly bool _arbitraryOrder;
    private bool[]? _matches;


    public PhrasePatternElement(string phrase, StringComparison? stringComparison = null, bool arbitraryOrder = false)
    {
        _phrase = phrase;
        _stringComparison = stringComparison ?? StringComparison.OrdinalIgnoreCase;
        _arbitraryOrder = arbitraryOrder;
    }

    public override bool Match(BotInput input, List<string> extracted, Tokenizer tokenizer)
    {
        _phraseTokens ??= tokenizer.Tokenize(_phrase).ToTokenList();
        var isMatch = true;
        if (_arbitraryOrder)
        {
            _matches ??= new bool[_phraseTokens.Count];
            for (var i = 0; i < _matches.Length; i++)
            {
                _matches[i] = false;
            }
        }

        foreach (var token in _phraseTokens)
        {
            if (!input.MoveNext())
            {
                return false;
            }

            isMatch = token.ValueAsSpan.Equals(input.Current.ValueAsSpan, _stringComparison);
            if (!isMatch && _arbitraryOrder)
            {
                isMatch = FindArbitraryMatch(input.Current);
            }

            if (!isMatch)
            {
                break;
            }
        }

        return isMatch;
    }

    private bool FindArbitraryMatch(IToken inputToken)
    {
        for (var i = 0; i < _phraseTokens?.Count; i++)
        {
            if (_matches[i])
            {
                continue;
            }

            var token = _phraseTokens[i];
            var isMatch = token.ValueAsSpan.Equals(inputToken.ValueAsSpan, _stringComparison);
            if (isMatch)
            {
                _matches![i] = true;
                return true;
            }
        }

        return false;
    }
}