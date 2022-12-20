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

        Gambit("SOURCE_PATTERN").WithPattern(x =>
                x.Word("I", "we").Word("am", "are").Word("at", "in").Wildcard(matchingResult => _source = matchingResult[0]))
            .WithOutput(() => $"Ok you want to go to {_target}");

        Gambit("TARGET")
            .WithPattern(x => x.Word("I").Word("want").Phrase("to go to").Wildcard(patternMatchingResult => _target = patternMatchingResult[0]))
            .WithOutput(() => $"Ok you want to go to {_target}");

        Gambit()
            .WithRegexPattern("What do I want")
            .When(x => _source is not null && _target is not null)
            .WithOutput(() => $"You want to go from {_source} to {_target}");
    }
}