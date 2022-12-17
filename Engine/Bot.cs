namespace Engine;

public class Bot
{
    public Language Language { get; set; }

    public List<BotRule> BotRules { get; set; } = new();

    public string ChatCompleteMessage { get; set; } = "Chat is completed";


    public RuleBuilder Gambit(string name = null)
    {
        var rule = new BotRule();
        BotRules.Add(rule);
        return new RuleBuilder(rule);
    }

    public string OneOf(params string[] words)
    {
        var index = new Random().Next(0, words.Length);
        return words[index];
    }
}