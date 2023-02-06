# Fluency 

Rule based chat bot engine inspired by  [ChatScript](https://github.com/ChatScript/ChatScript)

## ✨ Getting Started

Our bot will try to understand where the user is and where he wants to be, and will try to supply
useful information.

- Create a class representing conversation context:

```csharp
public class TravelBotContext
{
    /// <summary>
    /// Current location of the user
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Place where the user wants to go
    /// </summary>
    public string? Target { get; set; }
}
```

- Create a class representing the bot:

```csharp
public class TravelBot : Bot<TravelBotContext>
{
}


```

- Define rules in the constructor

```csharp
      R("Hello")
            .Pattern(x => x.Word(greetings))
            .Output((c) =>
                $"{OneOf("Hello", "Hi", "Hey")}. {OneOf("tell", "say")} me where you are and where do you want to {OneOf("travel", "go")}")
            .Keep()
            .Repeat();

        R("SOURCE")
            .Pattern(x =>
                x.Word("I", "we").Word("am", "are").Word("at", "in").Wildcard())
            .Then((x, m) => x.Source = m[0])
            .Output((c) => $"Ok you are in {c.Source}");

        R("TARGET")
            .Pattern(x =>
                x.Word("I", "we").Word("want").Phrase("to go to").Wildcard())
            .Then((x, m) => x.Target = m[0])
            .Output(c => $"Ok you want to go to {c.Target}");

        R("ASK_SOURCE")
            .When((x, i) => x.Source is null)
            .Output("Where are you?");

        R("ASK_DESTINATION")
            .When((x, i) => x.Target is null)
            .Output("Where do you want to go?");

        R("CONFIRMATION")
            .When((x, i) => x.Source is not null && x.Target is not null)
            .Pattern(x => x.Word("What").Word("do").Word("I", "we").Word("want", "desire"))
            .Output((c) => $"You want to go from {c.Source} to {c.Target}"); 
```