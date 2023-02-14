using Catalyst;

namespace Fluency.Engine.Tokenization;

public class BotInput
{
    private int _currentToken;
    private int _currentSentence;

    private Dictionary<string, object>? Variables { get; }

    public Document? ProcessedInput { get; set; }

    public string RawInput { get; }

    public BotInput(string rawInput, Dictionary<string, object>? variables = null)
    {
        _currentToken = -1;
        _currentSentence = 0;
        RawInput = rawInput;
        Variables = variables;
    }

    public bool MoveNext()
    {
        if (CanMoveNext())
        {
            _currentToken++;
            return true;
        }

        return false;
    }

    public bool CanMoveNext()
    {
        return ProcessedInput is not null && _currentToken < ProcessedInput[_currentSentence].TokensCount - 1;
    }

    public void Reset()
    {
        _currentToken = -1;
    }

    public IToken Current => ProcessedInput[_currentSentence][_currentToken];

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