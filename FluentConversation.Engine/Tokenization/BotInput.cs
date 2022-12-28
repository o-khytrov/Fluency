using System.Collections;
using Catalyst;

namespace FluentConversation.Engine.Tokenization;

public class BotInput
{
    private int _current;
    private readonly Dictionary<string, object>? _variables;

    public List<IToken>? Document { get; set; }

    public string RawInput { get; }

    public BotInput(string rawInput, Dictionary<string, object>? variables = null)
    {
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
        return _current < Document.Count - 1;
    }

    public void Reset()
    {
        _current = -1;
    }

    public IToken Current => Document[_current];

    public void Dispose()
    {
    }
}