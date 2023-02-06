namespace FluentConversation.Engine.PatternSystem.Elements;

public class PatternMatchingResult
{
    public bool Match { get; }

    private List<string> Extracted { get; }

    public string? this[int i] => Extracted.Count > i ? Extracted[i] : null;


    public PatternMatchingResult(bool result, List<string> extracted)
    {
        Match = result;
        Extracted = extracted;
    }
}