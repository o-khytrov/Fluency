using Catalyst;
using FluentConversation.Engine.Tokenization;

namespace FluentConversation.Engine.PatternSystem.Elements;

public class PhrasePatternElement : PatternElement
{
    private List<IToken>? _phraseTokens;

    private readonly StringComparison _stringComparison;
    private readonly string _phrase;

    public PhrasePatternElement(string phrase, StringComparison? stringComparison = null)
    {
        _phrase = phrase;
        _stringComparison = stringComparison ?? StringComparison.OrdinalIgnoreCase;
    }

    public override bool Match(BotInput input, List<string> extracted, Tokenizer tokenizer)
    {
        _phraseTokens ??= tokenizer.Tokenize(_phrase).ToTokenList();

        var isMatch = true;
        foreach (var value in _phraseTokens)
        {
            if (!input.MoveNext())
            {
                return false;
            }

            isMatch = value.ValueAsSpan.Equals(input.Current.ValueAsSpan, _stringComparison);
            if (!isMatch)
            {
                break;
            }
        }

        return isMatch;
    }
}