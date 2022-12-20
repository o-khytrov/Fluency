using System.Collections;

namespace Engine.Tokenization;

public class TokenCollection : IEnumerator<string>
{
    private readonly string[] _tokens;

    private int _current;

    public string RawInput { get; }

    public TokenCollection(string rawInput)
    {
        _tokens = Tokenizer.TokenizeStrings(rawInput);
        _current = -1;
        RawInput = rawInput;
    }

    public bool MoveNext()
    {
        if (CanMoveNext())
        {
            _current++;
            return true;
        }

        return false;
    }

    public bool CanMoveNext()
    {
        return _current < _tokens.Length - 1;
    }

    public void Reset()
    {
        _current = -1;
    }

    public string Current => _tokens[_current];

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}