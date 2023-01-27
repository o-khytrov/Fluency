using System.Collections;
using Catalyst;

namespace FluentConversation.Engine.Tokenization;

public class BotInput
{
    private int _current;

    public Dictionary<string, object>? Variables { get; }

    public List<IToken>? Document { get; set; }

    public string RawInput { get; }

    public BotInput(string rawInput, Dictionary<string, object>? variables = null)
    {
        _current = -1;
        RawInput = rawInput;
        Variables = variables;
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

    public object? TryGetVariable(string key, object? defaultValue = null)
    {
        if (Variables is not null && Variables.ContainsKey(key))
        {
            return Variables[key];
        }

        return defaultValue;
    }

    public T? TryGetVariable<T>(string key)
    {
        var readData = TryGetVariable(key);
        if (readData is T data)
        {
            return data;
        }

        try
        {
            return (T) Convert.ChangeType(readData, typeof(T));
        }
        catch (InvalidCastException)
        {
            return default;
        }
    }
}