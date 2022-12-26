using FluentConversation.Engine.PatternSystem.Elements;

namespace FluentConversation.Engine.PatternSystem;

public class DisjunctionBuilder
{
    private readonly Disjunction _disjunction;

    public DisjunctionBuilder()
    {
        _disjunction = new Disjunction();
    }

    public PatternBuilder Add()
    {
        return new PatternBuilder();
    }

    public Disjunction Build()
    {
        return _disjunction;
    }
}