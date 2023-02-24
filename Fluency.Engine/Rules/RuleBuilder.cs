using Fluency.Engine.PatternSystem;
using Fluency.Engine.PatternSystem.Elements;
using Fluency.Engine.Tokenization;

namespace Fluency.Engine.Rules;

public class RuleBuilder<T>
    : IRuleBuilderInitialStage<T>,
        IRuleBuilderFinalStage<T>,
        IRuleBuilderOutputStage<T>,
        IIntermediateStageBuilder<T>,
        IGambitBuilder<T>
    where T : ChatContext, new()
{
    private readonly BotRule<T> _botRule;
    private readonly Topic<T> _topic;


    public RuleBuilder(BotRule<T> botRule, Topic<T> parent)
    {
        _botRule = botRule;
        _topic = parent;
    }

    public IRuleBuilderFinalStage<T> Output(string output)
    {
        _botRule.SetOutputRenderer((x) => output);
        return this;
    }

    public IRuleBuilderFinalStage<T> Output(Func<T, string> outputRenderer)
    {
        _botRule.SetOutputRenderer(outputRenderer);
        return this;
    }

    public IRuleBuilderOutputStage<T> Pattern(Action<PatternBuilder<T>> patternBuilderAction)
    {
        var builder = new PatternBuilder<T>();
        patternBuilderAction.Invoke(builder);
        _botRule.Pattern = builder.Build();
        return this;
    }

    public IRuleBuilderOutputStage<T> Pattern(Func<Conversation<T>, PatternEngine, bool> patternBuilderAction)
    {
        _botRule.Condition = patternBuilderAction;
        return this;
    }

    public IIntermediateStageBuilder<T> When(Func<T, BotInput, bool> condition)
    {
        _botRule.Conditions.Add(condition);
        return this;
    }

    public IIntermediateStageBuilder<T> When(Func<Conversation<T>, BotInput, bool> condition)
    {
        _botRule.ConversationConditions.Add(condition);
        return this;
    }

    public IIntermediateStageBuilder<T> Do(Action<BotInput, T> action)
    {
        _botRule.PreActions.Add(action);
        return this;
    }

    public IRuleBuilderOutputStage<T> Then(Action<T, PatternMatchingResult> postAction)
    {
        _botRule.PostActions.Add(postAction);
        return this;
    }

    public RuleBuilder<T> Output(Func<string> outputRenderer)
    {
        return this;
    }


    public RuleBuilder<T> WithTest(string input, string expectedResponse)
    {
        _botRule.Tests.Add(new RuleTest() { Input = input, ExpectedResponse = expectedResponse });
        return this;
    }

    public void NexTopic(string topicName)
    {
        _botRule.NexTopic = topicName;
    }

    public void Rejoinder(Action action)
    {
        var dependencyContainer = new List<BotRule<T>>();
        // Capture any rules added to the parent rule inside this delegate.
        using (_topic.BotRules.Capture(dependencyContainer.Add))
        {
            action();
        }

        _botRule.Rejoinders.AddRange(dependencyContainer);
    }
}

public interface IConditionBuilder<T>
    where T : ChatContext, new()
{
    /// <summary>
    /// Defines an action which is always executed before evaluating precondition and pattern
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    IIntermediateStageBuilder<T> Do(Action<BotInput, T> action);

    /// <summary>
    /// Sets a precondition under which the rule will be executed is checked before the pattern is evaluated
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    IIntermediateStageBuilder<T> When(Func<T, BotInput, bool> condition);

    /// <summary>
    /// Sets a precondition under which the rule will be executed is checked before the pattern is evaluated
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    IIntermediateStageBuilder<T> When(Func<Conversation<T>, BotInput, bool> condition);
}

public interface IRuleBuilderInitialStage<T> : IConditionBuilder<T>
    where T : ChatContext, new()
{
    /// <summary>
    /// Builds a pipeline for pattern scanner
    /// </summary>
    /// <param name="patternBuilderAction"></param>
    /// <returns></returns>
    IRuleBuilderOutputStage<T> Pattern(Action<PatternBuilder<T>> patternBuilderAction);

    IRuleBuilderOutputStage<T> Pattern(Func<Conversation<T>, PatternEngine, bool> patternBuilderAction);
}

public interface IRuleBuilderFinalStage<T>
{
    /// <summary>
    /// Add nested rules aka 'Rejoinders'. They are used to anticipate how a user might respond to output and give direct feedback based on that response
    /// </summary>
    /// <param name="action"></param>
    public void Rejoinder(Action action);

    public void NexTopic(string topicName);
}

public interface IRuleBuilderOutputStage<T>
    where T : ChatContext, new()
{
    /// <summary>
    /// Set post action, which will be executed after if pattern matches
    /// </summary>
    /// <param name="postAction"></param>
    /// <returns></returns>
    IRuleBuilderOutputStage<T> Then(Action<T, PatternMatchingResult> postAction);

    /// <summary>
    /// Sets static output
    /// </summary>
    /// <param name="output"></param>
    /// <returns></returns>
    IRuleBuilderFinalStage<T> Output(string output);

    /// <summary>
    /// Sets function for rendering output
    /// </summary>
    /// <param name="outputRenderer"></param>
    /// <returns></returns>
    IRuleBuilderFinalStage<T> Output(Func<T, string> outputRenderer);
}

public interface IIntermediateStageBuilder<T>
    where T : ChatContext, new()
{
    /// <summary>
    /// Builds a pipeline for pattern scanner
    /// </summary>
    /// <param name="patternBuilderAction"></param>
    /// <returns></returns>
    IRuleBuilderOutputStage<T> Pattern(Action<PatternBuilder<T>> patternBuilderAction);

    /// <summary>
    /// Sets static output
    /// </summary>
    /// <param name="output"></param>
    /// <returns></returns>
    IRuleBuilderFinalStage<T> Output(string output);

    /// <summary>
    /// Sets function for rendering output
    /// </summary>
    /// <param name="outputRenderer"></param>
    /// <returns></returns>
    IRuleBuilderFinalStage<T> Output(Func<T, string> outputRenderer);
}

public interface IGambitBuilder<T> : IRuleBuilderOutputStage<T>, IConditionBuilder<T>
    where T : ChatContext, new()
{
}