using Engine;

namespace TravelBot;

public class TravelBot : Bot
{
    public Language Language => Language.English;


    private string? _target;
    private string? _source;


    private Concept _travel = new()
    {
        "journey", "trip", "ride"
    };

    public TravelBot()
    {
        Gambit("Hello")
            .WithRegexPattern("Hello")
            .WithOutput(() => $"{OneOf("Hello", "Hi", "Hey")}. {OneOf("talk", "speak", "say something")} to me")
            .Keep()
            .Repeat();

        Gambit("SOURCE").WithPattern(x =>
                x.Word("I", "we").Word("am", "are").Word("at", "in").Wildcard(w => _source = w))
            .WithOutput(() => $"Ok you are in {_source}");

        Gambit("TARGET")
            .WithPattern(x =>
                x.Word("I", "we").Word("want").Phrase("to go to").Wildcard(w => _target = w))
            .WithOutput(() => $"Ok you want to go to {_target}");

        Gambit("CONFIRMATION")
            .WithPattern(x => x.Word("What").Word("do").Word("I", "we").Word("want", "desire"))
            .When(x => _source is not null && _target is not null)
            .WithOutput(() => $"You want to go from {_source} to {_target}");
    }
}