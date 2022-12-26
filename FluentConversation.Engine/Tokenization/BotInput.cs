using System.Collections;

namespace FluentConversation.Engine.Tokenization;

public class BotInput : IEnumerator<string>
{
    private readonly string[] _tokens;

    private int _current;
    private readonly Dictionary<string, object>? _variables;

    public string RawInput { get; }

    public BotInput(string rawInput, Dictionary<string, object>? variables = null)
    {
        _tokens = Tokenizer.TokenizeStrings(rawInput);
        _current = -1;
        RawInput = rawInput;
        _variables = variables;
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