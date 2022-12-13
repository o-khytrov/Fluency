namespace ConversationDesigner;

public class MyBot : Bot
{
    public Language Language => Language.English;

    public MyBot()
    {
        BotRules = new List<BotRule>()
        {
            new((x) => true, () => $"{OneOf("Hello", "Hi", "Hey")}. {OneOf("talk", "speak", "say something")} to me")
        };
    }

    public string OneOf(params string[] words)
    {
        var index = new Random().Next(0, words.Length);
        return words[index];
    }
}