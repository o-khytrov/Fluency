namespace ConversationDesigner;

public class TravelBot : Bot
{
    public Language Language => Language.English;


    private string? _target;
    private string? _source;

    public TravelBot()
    {
        Gambit("Hello")
            .WithPattern("Hello")
            .WithOutput(() => $"{OneOf("Hello", "Hi", "Hey")}. {OneOf("talk", "speak", "say something")} to me")
            .Keep()
            .Repeat();

        Gambit("SOURCE")
            .When(x => string.IsNullOrEmpty(_source))
            .WithPattern(@"I am at (\w*)", x => { _source = x.First().Groups[1].Value; })
            .WithOutput(() => $"Ok you are at {_source}");

        Gambit("TARGET")
            .WithPattern(@"I want to go to (\w*)", x => { _target = x.First().Groups[1].Value; })
            .WithOutput(() => $"Ok you want to go to {_target}");

        Gambit()
            .WithPattern("What do I want")
            .When(x => _source is not null && _target is not null)
            .WithOutput(() => $"You want to go from {_source} to {_target}");
    }
}