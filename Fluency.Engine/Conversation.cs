using System.Text;
using Fluency.Engine.Models;
using Fluency.Engine.Rules;

namespace Fluency.Engine;

public class Conversation<T>
    where T : ChatContext, new()
{
    public T Context { get; set; } = new();

    public List<BotRule<T>> RuleShown { get; set; } = new();

    public List<BotRule<T>> PendingRejoinders { get; set; } = new();

    public List<Message> Messages { get; set; } = new();

    public string UserId { get; set; } = string.Empty;

    public string CurrentTopic { get; set; }

    public int Input { get; set; }

    private List<Volley> Volleys { get; set; } = new();

    internal Volley CurrentVolley { get; set; }

    public IDisposable BeginVolley()
    {
        CurrentVolley = new Volley();
        Volleys.Add(CurrentVolley);
        return new VolleyScope();
    }

    class VolleyScope : IDisposable
    {
        public void Dispose()
        {
        }
    }
}

class Volley
{
    private readonly StringBuilder _stringBuilder;

    public int ResponseCount { get; private set; }

    public Volley()
    {
        _stringBuilder = new StringBuilder();
    }

    public void AddOutput(string output)
    {
        ResponseCount++;
        _stringBuilder.Append(output);
    }

    public string GetResponse()
    {
        return _stringBuilder.ToString();
    }
}